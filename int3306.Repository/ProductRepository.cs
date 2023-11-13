using int3306.Entities;
using int3306.Repository.Models;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(DataContext dataContext) : base(dataContext) {}

        public override async Task<IBaseResult<Product>> Get(int id)
        {
            try
            {
                var r = await DataContext.GetDbSet<Product>()
                    .Include(product => product.ProductType)
                    .Include(product => product.ProductToTags.Where(pt => pt.Status > 0))
                    .ThenInclude(tag => tag.ProductTag)
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (r != null)
                {
                    r.ProductTags = r.ProductToTags.Select(r => r.ProductTag).ToList();
                }
                return r != null ? BaseResult<Product>.FromSuccess(r) : BaseResult<Product>.FromNotFound();
            }
            catch (Exception e)
            {
                return BaseResult<Product>.FromError(e.ToString());
            }
        }

        public override async Task<IBaseResult<List<Product>>> List(bool inUse = true)
        {
            try
            {
                var a = (IQueryable<Product>) DataContext.GetDbSet<Product>()
                    .Include(product => product.ProductType)
                    .Include(product => product.ProductToTags.Where(pt => pt.Status > 0))
                    .ThenInclude(tag => tag.ProductTag);

                if (inUse)
                {
                    a = a.Where(entity => entity.Status > 0);
                }

                var result = await a.ToListAsync();
                result = result.Select(r =>
                {
                    r.ProductTags = r.ProductToTags.Select(r => r.ProductTag).ToList();
                    return r;
                }).ToList();
                return BaseResult<List<Product>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<Product>>.FromError(e.ToString());
            }
        }

        public async Task<IBaseResult<List<Product>>> Search(ProductSearchModel model)
        {
            try
            {
                var query = (IQueryable<Product>)DataContext.GetDbSet<Product>()
                    .Include(product => product.ProductType)
                    .Include(product => product.ProductToTags.Where(pt => pt.Status > 0))
                    .ThenInclude(tag => tag.ProductTag);

                if (model.ProductType > 0)
                {
                    query = query.Where(p => p.ProductTypeId == model.ProductType);
                }

                var tags = model.ProductTags ?? new List<int>();
                if (tags.Count != 0)
                {
                    query = query.Where(q => q.ProductToTags.Any(ptt => tags.Contains(ptt.ProductTagId)));
                }

                if (model.MinPrice.HasValue) query = query.Where(q => q.Price >= model.MinPrice);
                if (model.MaxPrice.HasValue) query = query.Where(q => q.Price <= model.MaxPrice);

                if (model.GrowingSeasonMin.HasValue) query = query.Where(q => q.GrowingSeason >= model.GrowingSeasonMin);
                if (model.GrowingSeasonMax.HasValue) query = query.Where(q => q.GrowingSeason <= model.GrowingSeasonMax);

                query = query.Where(p => p.Status > 0);
                
                var result = await query.ToListAsync();
                result = result.Select(r =>
                {
                    r.ProductTags = r.ProductToTags.Select(r => r.ProductTag).ToList();
                    return r;
                }).ToList();
                return BaseResult<List<Product>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<Product>>.FromError(e.ToString());
            }
            
            
        }
    }
}