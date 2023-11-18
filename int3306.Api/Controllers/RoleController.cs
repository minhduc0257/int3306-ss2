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
    public class RoleController : BaseController<Role>
    {
        public RoleController(IBaseRepository<Role> repository) : base(repository) {}
    }
}