using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class CartRepository : BaseRepository<Cart>
    {
        public CartRepository(DataContext dataContext) : base(dataContext) {}

        public override async Task<IBaseResult<int>> Post(Cart entity)
        {
            entity.Added = DateTime.Now;
            var db = DataContext.GetDbSet<Cart>();
            try
            {
                await DataContext.Database.BeginTransactionAsync();

                var existing = await db
                    .Where(c => c.Status > 0 && c.ProductId == entity.ProductId && c.UserId == entity.UserId)
                    .FirstOrDefaultAsync();

                if (existing == null)
                {
                    entity.Id = 0;
                    entity.Status = 1;
                    await db.AddAsync(entity);
                    await DataContext.SaveChangesAsync();   
                }
                else
                {
                    existing.Count += entity.Count;
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