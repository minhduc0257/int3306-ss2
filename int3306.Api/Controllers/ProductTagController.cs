using int3306.Api.Structures;
using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Controllers
{
    [Route("[controller]")]
    public class ProductTagController : BaseController<ProductTag>
    {
        public ProductTagController(IBaseRepository<ProductTag> repository) : base(repository) {}
    }
}