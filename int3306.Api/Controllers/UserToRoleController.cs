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
    public class UserToRoleController : BaseController<UserToRole>
    {
        private readonly UserToRoleRepository repository;

        public UserToRoleController(UserToRoleRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        
        [Route("ListByUserId/{userId:int}")]
        [HttpGet]
        public async Task<ActionResult<IBaseResult<List<UserToRole>>>> ListByUserId(int userId)
        {
            var r = await repository.ListByUserId(userId);
            return ResultResponse(r);
        }

        public override async Task<ActionResult<IBaseResult<List<UserToRole>>>> List()
        {
            var r = await repository.ListByUserId(GetUserId()!.Value);
            return ResultResponse(r);
        }
    }
}