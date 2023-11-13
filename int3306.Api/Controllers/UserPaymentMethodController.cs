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
    public class UserPaymentMethodController : BaseController<UserPaymentMethod>
    {
        public UserPaymentMethodController(IBaseRepository<UserPaymentMethod> repository) : base(repository) {}
    }
}