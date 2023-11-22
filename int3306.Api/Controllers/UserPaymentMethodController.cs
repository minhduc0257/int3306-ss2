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
    public class UserPaymentMethodController : BaseController<UserPaymentMethod>
    {
        private readonly UserPaymentMethodRepository repository;
        public UserPaymentMethodController(UserPaymentMethodRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        
        public override async Task<ActionResult<IBaseResult<List<UserPaymentMethod>>>> List()
        {
            return ResultResponse(await repository.ListByUserId(GetUserId()!.Value));
        }

        public override Task<ActionResult<IBaseResult<UserPaymentMethod>>> Post(UserPaymentMethod payload)
        {
            payload.UserId = GetUserId()!.Value;
            return base.Post(payload);
        }
    }
}