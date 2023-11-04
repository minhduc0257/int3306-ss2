using int3306.Api.Structures;
using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductTypeController : BaseController<ProductType>
    {
        public ProductTypeController(IBaseRepository<ProductType> repository) : base(repository) {}
    }
}