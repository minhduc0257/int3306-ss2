using int3306.Api.Structures;
using int3306.Entities;
using int3306.Repository;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Mvc;

namespace int3306.Api.Controllers
{
    [Route("[controller]")]
    public class AssetController : BaseController<Asset>
    {
        private AssetRepository repository;
        public AssetController(AssetRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public override Task<ActionResult<IBaseResult<Asset>>> Post(Asset payload)
        {
            return Task.FromResult<ActionResult<IBaseResult<Asset>>>(BaseResult<Asset>.FromError("Use CreateFile instead"));
        }

        [Route("CreateFile")]
        [HttpPost]
        public async Task<ActionResult<IBaseResult<Asset>>> CreateFile([FromHeader(Name = "name")] string? name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "";
            }

            if (!Request.Body.CanSeek)
            {
                Request.EnableBuffering();
            }

            using var memstream = new MemoryStream();
            await Request.Body.CopyToAsync(memstream);

            var entity = new Asset
            {
                File = memstream.ToArray(),
                Name = name
            };

            return await Post(entity);
        }
    }
}