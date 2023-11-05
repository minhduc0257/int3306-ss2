using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class ProductToTagRepository : BaseRepository<ProductToTag>
    {
        public ProductToTagRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<IBaseResult<List<ProductToTag>>> GetByProductId(int productId)
        {
            try
            {
                var r = await DataContext.GetDbSet<ProductToTag>()
                    .Where(ptt => ptt.ProductId == productId)
                    .ToListAsync();
                return BaseResult<List<ProductToTag>>.FromSuccess(r);
            }
            catch (Exception e)
            {
                return BaseResult<List<ProductToTag>>.FromError(e.ToString());
            }
        }

        public async Task<IBaseResult<List<ProductToTag>>> BulkAdd(int productId, List<int> tagId)
        {
            try
            {
                var db = DataContext.GetDbSet<ProductToTag>();
                var entries = tagId.Select(t => new ProductToTag
                {
                    ProductTagId = t,
                    ProductId = productId
                }).ToList();
                await db.AddRangeAsync(entries);
                await DataContext.SaveChangesAsync();

                return BaseResult<List<ProductToTag>>.FromSuccess(entries);
            }
            catch (Exception e)
            {
                return BaseResult<List<ProductToTag>>.FromError(e.ToString());
            }
        }
    }
}