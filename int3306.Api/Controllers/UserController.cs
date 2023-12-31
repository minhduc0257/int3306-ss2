using System.Net;
using int3306.Api.Structures;
using int3306.Entities;
using int3306.Repository;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("user")]
    public class UserController : BaseController<User>
    {
        private readonly UserRepository userRepository;
        public UserController(UserRepository userRepository) : base(userRepository)
        {
            this.userRepository = userRepository;
        }

        public override async Task<ActionResult<IBaseResult<User>>> Get(int id)
        {
            var r = await userRepository.GetWithDetail(id);
            return ResultResponse(r);
        }

        [HttpGet]
        [Route("self")]
        public async Task<ActionResult<BaseResult<User>>> GetSelf()
        {
            var u = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Id)?.Value;
            if (u == null || !int.TryParse(u, out var uid))
            {
                return NotFound(BaseResult<User>.FromNotFound()); 
            }

            var user = await userRepository.GetWithDetail(uid);
            return user.StatusCodeHint switch
            {
                null => Ok(user),
                _ => StatusCode((int)user.StatusCodeHint.Value, user)
            };
        }
    }
}