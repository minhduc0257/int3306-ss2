using int3306.Entities.Shared;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Structures
{
    public class BaseController<T> : Controller where T : IBaseEntity
    {
        private readonly IBaseRepository<T> repository;
        public BaseController(IBaseRepository<T> repository)
        {
            this.repository = repository;
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IBaseResult<T>>> Get(int id)
        {
            var r = await repository.Get(id);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
        
        [HttpPost]
        [Route("{id:int}")]
        public async Task<ActionResult<IBaseResult<T>>> Post([FromBody] T payload)
        {
            var r = await repository.Post(payload);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
        
        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IBaseResult<T>>> Put(int id, [FromBody] T payload)
        {
            var r = await repository.Put(id, payload);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IBaseResult<T>>> Delete(int id)
        {
            var r = await repository.Delete(id);
            return r.StatusCodeHint switch
            {
                null => r.Success ? Ok(r) : BadRequest(r),
                _ => StatusCode((int)r.StatusCodeHint.Value, r)
            };
        }
    }
}