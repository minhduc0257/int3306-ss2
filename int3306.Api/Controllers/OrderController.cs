using int3306.Api.Structures;
using int3306.Entities;
using int3306.Repository;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrderController : BaseController<Order>
    {
        private readonly CartRepository cartRepository;
        private readonly OrderRepository orderRepository;
        public OrderController(OrderRepository repository, CartRepository cartRepository) : base(repository)
        {
            this.cartRepository = cartRepository;
            orderRepository = repository;
        }

        public override async Task<ActionResult<IBaseResult<Order>>> Post(Order payload)
        {
            payload.UserId = GetUserId()!.Value;
            if (!(payload.CartId?.Count > 0))
            {
                return BaseResult<Order>.FromError("Order must reference at least one item in cart!");
            }

            var existentId = await cartRepository.ContainExistentId(payload.CartId);
            if (!existentId)
            {
                return BaseResult<Order>.FromError("Order must reference at least existent cart item!");
            }
            
            return await base.Post(payload);
        }

        public override async Task<ActionResult<IBaseResult<List<Order>>>> List()
        {
            var res = await orderRepository.ListByUserId(GetUserId()!.Value);
            return ResultResponse(res);
        }

        [HttpGet]
        [RequirePermission(PermissionIndex.Admin)]
        [Route("ListAll")]
        public async Task<ActionResult<IBaseResult<List<Order>>>> ListAll()
        {
            return await base.List();
        }
    }
}