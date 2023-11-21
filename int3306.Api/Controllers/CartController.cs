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
    public class CartController : BaseController<Cart>
    {
        private readonly IBaseRepository<Product> productRepository;
        private readonly CartRepository cartRepository;
        public CartController(CartRepository repository, IBaseRepository<Product> productRepository) : base(repository)
        {
            this.productRepository = productRepository;
            cartRepository = repository;
        }

        public override async Task<ActionResult<IBaseResult<Cart>>> Post(Cart payload)
        {
            var product = await productRepository.Get(payload.ProductId);
            if (!product.Success)
            {
                return BaseResult<Cart>.FromError(product.Error);
            }

            if (payload.Count <= 0)
            {
                return BaseResult<Cart>.FromError("Count must be greater than 0!");
            }
            payload.UserId = GetUserId()!.Value;
            return await base.Post(payload);
        }

        [RequirePermission(PermissionIndex.Admin)]
        public override async Task<ActionResult<IBaseResult<List<Cart>>>> List()
        {
            var res = await cartRepository.ListByUserId(GetUserId()!.Value);
            return ResultResponse(res);
        }
    }
}