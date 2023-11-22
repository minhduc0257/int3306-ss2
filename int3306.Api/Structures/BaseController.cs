using int3306.Entities.Shared;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Structures
{
    public class BaseController<T> : Controller where T : IBaseEntity
    {
        protected readonly IBaseRepository<T> Repository;
        public BaseController(IBaseRepository<T> repository)
        {
            Repository = repository;
        }

        protected int? GetUserId()
        {
            var u = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Id)?.Value;
            if (u == null || !int.TryParse(u, out var uid))
            {
                return null;
            }

            return uid;
        }

        protected bool GetIsAdmin()
        {
            return true;
        }

        [HttpGet]
        [Route("{id:int}")]
        public virtual async Task<ActionResult<IBaseResult<T>>> Get(int id)
        {
            var r = await Repository.Get(id);
            return ResultResponse(r);
        }
        
        [HttpGet]
        [Route("List")]
        public virtual async Task<ActionResult<IBaseResult<List<T>>>> List()
        {
            var r = await Repository.List();
            return ResultResponse(r);
        }
        
        [HttpPost]
        [Route("")]
        [Authorize]
        public virtual async Task<ActionResult<IBaseResult<T>>> Post([FromBody] T payload)
        {
            var r = await Repository.Post(payload);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
        
        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public virtual async Task<ActionResult<IBaseResult<T>>> Put(int id, [FromBody] T payload)
        {
            var r = await Repository.Put(id, payload);
            return ResultResponse(r);
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public virtual async Task<ActionResult<IBaseResult<T>>> Delete(int id)
        {
            var r = await Repository.Delete(id);
            return ResultResponse(r);
        }

        protected ActionResult<IBaseResult<TE>> ResultResponse<TE>(IBaseResult<TE> r)
        {
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
    }
}