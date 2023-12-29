using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.Extensions.Logging;

namespace int3306.Repository
{
    public class AssetRepository : BaseRepository<Asset>
    {
        private ILogger<AssetRepository> logger;
        private string path;

        public AssetRepository(DataContext dataContext, ILogger<AssetRepository> logger) : base(dataContext)
        {
            this.logger = logger;

            var p = Environment.GetEnvironmentVariable("DATA_PATH");
            if (!string.IsNullOrWhiteSpace(p))
            {
                path = p;
            }
            else
            {
                throw new Exception("DATA_PATH env var isn't set; assets cannot be served");
            }
        }

        public override async Task<IBaseResult<int>> Post(Asset entity)
        {
            var r = await base.Post(entity);
            if (!r.Success) return r;
            
            try
            {
                await File.WriteAllBytesAsync(
                    Path.Join(path, r.Data.ToString()),
                    entity.File
                );
            }
            catch (Exception e)
            {
                logger.LogError(e, "Couldn't write file to storage");
                return BaseResult<int>.FromError("Couldn't store file!");
            }

            return r;
        }

        public Stream GetFile(int id)
        {
            var s = File.Open(Path.Join(path, id.ToString()), FileMode.Open);
            return s;
        }
    }
}