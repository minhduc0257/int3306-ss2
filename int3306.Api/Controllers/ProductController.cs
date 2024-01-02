using System.Collections.Immutable;
using int3306.Api.Structures;
using int3306.Entities;
using int3306.Repository;
using int3306.Repository.Models;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : BaseController<Product>
    {
        private readonly ProductToTagRepository productToTagRepository;
        private readonly ProductRepository productRepository;
        public ProductController(ProductRepository repository, ProductToTagRepository ptt) : base(repository)
        {
            productToTagRepository = ptt;
            productRepository = repository;
        }
        
        [HttpPost]
        [Route("Find")]
        public async Task<IBaseResult<List<Product>>> Find([FromBody] ProductSearchModel searchModel)
        {
            var records = await productRepository.Search(searchModel);
            return records;
        }

        [HttpPut]
        [Authorize]
        [Route("SetTag/{productId:int}")]
        public async Task<ActionResult<BaseResult<bool>>> SetTag(int productId, [FromBody] List<int> tagIds)
        {
            var records = await productToTagRepository.GetByProductId(productId);
            if (!records.Success)
            {
                return Ok(records);
            }

            var list = records.Data!;
            var set = tagIds.ToImmutableHashSet();
            var toDelete = list
                .Where(r => !set.Contains(r.ProductTagId))
                .Select(record => record.Id);

            var toAdd = set.Where(id => list.All(r => r.ProductTagId != id)).ToList();

            var res = await productToTagRepository.BulkDelete(toDelete.ToList());
            if (toAdd.Count != 0)
            {
                await productToTagRepository.BulkAdd(productId, toAdd);
            }

            return res.Success ? BaseResult<bool>.FromSuccess(true) : BaseResult<bool>.FromError(res.Error);
        }

        [HttpGet]
        [Route("GetByType/{type:int}")]
        public async Task<ActionResult<IBaseResult<List<Product>>>> GetByType(int type)
        {
            var response = await productRepository.GetByType(type);
            return ResultResponse(response);
        }
    }
}