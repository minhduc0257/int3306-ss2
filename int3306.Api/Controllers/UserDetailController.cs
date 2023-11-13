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
    public class UserDetailController : BaseController<UserDetail>
    {
        private readonly UserDetailRepository repository;
        public UserDetailController(UserDetailRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public override Task<ActionResult<IBaseResult<UserDetail>>> Delete(int id)
        {
            return Task.FromResult<ActionResult<IBaseResult<UserDetail>>>(BaseResult<UserDetail>.FromError("Cannot delete user detail"));
        }
    }
}