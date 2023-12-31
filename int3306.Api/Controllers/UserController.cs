using System.Collections.Immutable;
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
        private readonly UserToRoleRepository userToRoleRepository;
        public UserController(UserRepository userRepository, UserToRoleRepository userToRoleRepository) : base(userRepository)
        {
            this.userRepository = userRepository;
            this.userToRoleRepository = userToRoleRepository;
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
        
        [HttpPut]
        [Authorize]
        [Route("SetRole/{userId:int}")]
        public async Task<ActionResult<IBaseResult<bool>>> SetRole(int userId, [FromBody] List<int> roleId)
        {
            var records = await userToRoleRepository.ListByUserId(userId);
            if (!records.Success)
            {
                return ResultResponse(BaseResult<List<UserToRole>>.IntoError<bool>(records));
            }

            var list = records.Data!;
            var set = roleId.ToImmutableHashSet();
            var toDelete = list
                .Where(r => !set.Contains(r.RoleId))
                .Select(record => record.Id);

            var toAdd = set.Where(id => list.All(r => r.RoleId != id)).ToList();

            var res = await userToRoleRepository.BulkDelete(toDelete.ToList());
            if (toAdd.Count != 0)
            {
                await userToRoleRepository.BulkAdd(userId, toAdd);
            }

            return res.Success ? BaseResult<bool>.FromSuccess(true) : BaseResult<bool>.FromError(res.Error);
        }
    }
}