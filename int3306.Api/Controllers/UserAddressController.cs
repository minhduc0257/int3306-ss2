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
    public class UserAddressController : BaseController<UserAddress>
    {
        private readonly UserAddressRepository userAddressRepository;
        public UserAddressController(UserAddressRepository repository) : base(repository)
        {
            userAddressRepository = repository;
        }

        public override async Task<ActionResult<IBaseResult<List<UserAddress>>>> List()
        {
            var res = await userAddressRepository.ListByUserId(GetUserId()!.Value);
            return ResultResponse(res);
        }

        public override Task<ActionResult<IBaseResult<UserAddress>>> Post(UserAddress payload)
        {
            payload.UserId = GetUserId()!.Value;
            return base.Post(payload);
        }

        public override Task<ActionResult<IBaseResult<UserAddress>>> Put(int id, UserAddress payload)
        {
            payload.UserId = GetUserId()!.Value;
            return base.Put(id, payload);
        }
    }
}