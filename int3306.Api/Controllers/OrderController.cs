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
        public OrderController(IBaseRepository<Order> repository, CartRepository cartRepository) : base(repository)
        {
            this.cartRepository = cartRepository;
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
    }
}