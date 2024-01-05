using int3306.Entities;
using int3306.Repository.Models;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(DataContext dataContext) : base(dataContext) {}

        private Task<Dictionary<int, int>> GetTotalOrder(List<int> ids)
        {
            var q = DataContext.ProductRating
                .FromSqlRaw(
                    "select product_id, sum(count) as count, 0.0 as rating from `order_detail` where status >= 1 group by product_id");

            q = q.Where(p => ids.Contains(p.ProductId));

            return q.ToDictionaryAsync(p => p.ProductId, p => p.Count);
        }

        private IQueryable<Product> GetBaseJoinedQuery(IQueryable<Product>? baseQuery = null)
        {
            return (baseQuery ?? DataContext.GetDbSet<Product>())
                .Include(product => product.ProductType)
                .Include(product => product.ProductToTags.Where(pt => pt.Status > 0))
                .ThenInclude(tag => tag.ProductTag)
                .Include(product => product.ProductThumbnails.Where(pt => pt.Status > 0))
                .Include(product => product.ProductVariants.Where(pv => pv.Status > 0))
                .ThenInclude(pv => pv.ProductVariantValues.Where(pvv => pvv.Status > 0))
                .Include(product => product.Stocks.Where(pt => pt.Status > 0))
                .AsSplitQuery();
        }
        
        public override async Task<IBaseResult<Product>> Get(int id)
        {
            try
            {
                var r = await GetBaseJoinedQuery().FirstOrDefaultAsync(e => e.Id == id);
                var order = await GetTotalOrder(new List<int> { id });
                if (r != null)
                {
                    r.ProductTags = r.ProductToTags.Select(r => r.ProductTag).ToList();

                    var rating = await DataContext.GetDbSet<OrderDetail>()
                        .Where(p => p.ProductId == r.Id)
                        .Select(d => d.Rating)
                        .ToListAsync();

                    r.Rating = rating.Count == 0 ? 0 : (float)rating.Average();
                    r.TotalOrder = order.GetValueOrDefault(id, 0);
                    r.Stock = r.Stocks.Count == 0 ? 0 : r.Stocks.Select(s => s.Count).Sum();
                    r.Stock -= r.TotalOrder;
                    r.Stock = Math.Max(r.Stock, 0);
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
                var ratingQuery = DataContext.ProductRating
                    .FromSqlRaw("""
                                select product_id, rating, 0 as count from
                                (
                                    select product_id, AVG(rating) as rating from order_detail
                                    group by product_id
                                ) pr
                                    join `products` p
                                         on p.id = pr.product_id
                                """ + (inUse ? " where p.status > 0" : ""));

                var a = GetBaseJoinedQuery();

                if (inUse)
                {
                    a = a.Where(entity => entity.Status > 0);
                }

                a = a.AsSplitQuery();
                var result = await a.ToListAsync();
                var rating = await ratingQuery.ToDictionaryAsync(pair => pair.ProductId, pair => pair.Rating);
                var order = await GetTotalOrder(result.Select(a => a.Id).ToList());
                result = result.Select(r =>
                {
                    r.ProductTags = r.ProductToTags.Select(r => r.ProductTag).ToList();
                    r.Stock = r.Stocks.Select(s => s.Count).Sum();
                    r.Rating = rating.GetValueOrDefault(r.Id, 0);
                    r.TotalOrder = order.GetValueOrDefault(r.Id, 0);
                    r.Stock = r.Stocks.Count == 0 ? 0 : r.Stocks.Select(s => s.Count).Sum();
                    r.Stock -= r.TotalOrder;
                    r.Stock = Math.Max(r.Stock, 0);
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
                var query = (IQueryable<Product>)DataContext.GetDbSet<Product>();
                if (!string.IsNullOrWhiteSpace(model.Query))
                {
                    query = DataContext.GetDbSet<Product>()
                        .FromSql($"select * from products where name LIKE {"%" + model.Query + "%"}");
                }

                query = GetBaseJoinedQuery(query);

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
                query = query.AsSplitQuery();
                
                var ratingQuery = DataContext.ProductRating
                    .FromSqlRaw("""
                                select product_id, rating, 0 as count from
                                (
                                    select product_id, AVG(rating) as rating from order_detail
                                    group by product_id
                                ) pr
                                    join `products` p
                                         on p.id = pr.product_id
                                """ + (true ? " where p.status > 0" : ""));
                
                var result = await query.ToListAsync();
                var order = await GetTotalOrder(result.Select(a => a.Id).ToList());
                var ids = result.Select(r => r.Id).ToList();
                var rating = await ratingQuery
                    .Where(p => ids.Contains(p.ProductId))
                    .ToDictionaryAsync(pair => pair.ProductId, pair => pair.Rating);
                result = result.Select(r =>
                {
                    r.ProductTags = r.ProductToTags.Select(r => r.ProductTag).ToList();
                    r.Rating = rating.GetValueOrDefault(r.Id, 0);
                    r.TotalOrder = order.GetValueOrDefault(r.Id, 0);
                    r.Stock = r.Stocks.Count == 0 ? 0 : r.Stocks.Select(s => s.Count).Sum();
                    r.Stock -= r.TotalOrder;
                    r.Stock = Math.Max(r.Stock, 0);
                    return r;
                }).ToList();
                return BaseResult<List<Product>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<Product>>.FromError(e.ToString());
            }
        }

        public async Task<IBaseResult<List<Product>>> GetByType(int type)
        {
            var query = GetBaseJoinedQuery();
            query = type switch
            {
                1 => query.OrderByDescending(p => p.Id),
                2 => query.OrderByDescending(p => p.Name),
                _ => query.OrderByDescending(p => p.Description)
            };
            query = query.Take(20);

            var result = await query.ToListAsync();
            return BaseResult<List<Product>>.FromSuccess(result);
        }
    }
}