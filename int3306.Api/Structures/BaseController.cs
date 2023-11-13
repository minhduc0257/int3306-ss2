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
        
        [HttpGet]
        [Route("{id:int}")]
        public virtual async Task<ActionResult<IBaseResult<T>>> Get(int id)
        {
            var r = await Repository.Get(id);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
        
        [HttpGet]
        [Route("List")]
        public async Task<ActionResult<IBaseResult<T>>> List()
        {
            var r = await Repository.List();
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
        
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<IBaseResult<T>>> Post([FromBody] T payload)
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
        public async Task<ActionResult<IBaseResult<T>>> Put(int id, [FromBody] T payload)
        {
            var r = await Repository.Put(id, payload);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public virtual async Task<ActionResult<IBaseResult<T>>> Delete(int id)
        {
            var r = await Repository.Delete(id);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
    }
}