using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class CartRepository : BaseRepository<Cart>
    {
        public CartRepository(DataContext dataContext) : base(dataContext) {}

        public override Task<IBaseResult<int>> Post(Cart entity)
        {
            entity.Added = DateTime.Now;
            return base.Post(entity);
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