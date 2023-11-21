using int3306.Api.Structures;
using int3306.Entities;
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
        public OrderController(IBaseRepository<Order> repository) : base(repository) {}

        public override Task<ActionResult<IBaseResult<Order>>> Post(Order payload)
        {
            payload.UserId = GetUserId()!.Value;
            return base.Post(payload);
        }
    }
}