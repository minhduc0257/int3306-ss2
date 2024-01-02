using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class CartRepository : BaseRepository<Cart>
    {
        public CartRepository(DataContext dataContext) : base(dataContext) {}

        private async Task<IBaseResult<bool>> ValidateRequiredVariants(Cart entity)
        {
            var db = DataContext.GetDbSet<ProductVariant>();
            var pv = await db
                .Include(d => d.ProductVariantValues)
                .Where(d => d.ProductId == entity.ProductId)
                .ToListAsync();

            var mpv = pv.ToDictionary(
                pv => pv.Id,
                pv => pv.ProductVariantValues.Select(r => r.Id).ToList()
            );

            var existing = entity.CartVariants.ToDictionary(c => c.VariantId, c => c.VariantValueId);

            foreach (var pair in mpv)
            {
                if (!existing.ContainsKey(pair.Key))
                {
                    return BaseResult<bool>.FromError($"Variant id {pair.Key} not found!");
                }

                var value = existing[pair.Key];
                if (!pair.Value.Contains(value))
                {
                    return BaseResult<bool>.FromError($"Variant id {pair.Key} does not contain value id {value}!");
                }
            }
            
            return BaseResult<bool>.FromSuccess(true);
        }
        
        public override async Task<IBaseResult<int>> Post(Cart entity)
        {
            var validate = await ValidateRequiredVariants(entity);
            if (!validate.Success)
            {
                return BaseResult<bool>.IntoError<int>(validate);
            }
            
            entity.Added = DateTime.Now;
            var db = DataContext.GetDbSet<Cart>();
            try
            {
                await DataContext.Database.BeginTransactionAsync();

                var list = await db
                    .Include(c => c.CartVariants)
                    .Where(c => c.Status > 0 && c.ProductId == entity.ProductId && c.UserId == entity.UserId)
                    .ToListAsync();

                Cart? existing = null;
                
                var m1 = entity.CartVariants.ToDictionary(cv => cv.VariantId, cv => cv.VariantValueId);
                foreach (var e in list)
                {
                    var m2 = e.CartVariants.ToDictionary(cv => cv.VariantId, cv => cv.VariantValueId);

                    if (m1.Count != m2.Count)
                    {
                        continue;
                    }

                    var match = true;
                    
                    foreach (var (variant, value) in m1)
                    {
                        if (m2.GetValueOrDefault(variant, 0) != value)
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        existing = e;
                        break;
                    }
                }
                
                if (existing == null)
                {
                    entity.Id = 0;
                    entity.Status = 1;
                    await db.AddAsync(entity);
                    await DataContext.GetDbSet<CartVariant>().AddRangeAsync(entity.CartVariants);
                    await DataContext.SaveChangesAsync();   
                }
                else
                {
                    existing.Count += entity.Count;
                    entity = existing;
                    await DataContext.SaveChangesAsync();
                }

                await DataContext.Database.CommitTransactionAsync();

                return BaseResult<int>.FromSuccess(entity.Id);
            }
            catch (Exception e)
            {
                await DataContext.Database.RollbackTransactionAsync();
                return BaseResult<int>.FromError(e.ToString());
            }
        }

        public async Task<bool> ContainExistentId(List<int> ids)
        {
            var a = (IQueryable<Cart>) DataContext.GetDbSet<Cart>();
            var r = await a
                .Where(a => a.Status > 0)
                .CountAsync(a => ids.Contains(a.Id));

            return ids.Count == r;
        }
        
        public async Task<IBaseResult<List<Cart>>> ListByUserId(int userId)
        {
            try
            {
                var a = (IQueryable<Cart>) DataContext.GetDbSet<Cart>();
                a = a
                    .Where(entity => entity.Status > 0)
                    .Where(entity => entity.UserId == userId);

                a = a
                    .Include(c => c.Product)
                    .ThenInclude(
                        p => p!.ProductThumbnails
                            .Where(pt => pt.Status > 0)
                            .OrderByDescending(pt => pt.Priority)
                            .Take(1)
                    )
                    .AsSplitQuery();

                var result = await a.ToListAsync();
                return BaseResult<List<Cart>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<Cart>>.FromError(e.ToString());
            }
        }
    }
}