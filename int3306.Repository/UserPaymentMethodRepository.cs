using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class UserPaymentMethodRepository : BaseRepository<Entities.UserPaymentMethod>
    {
        public UserPaymentMethodRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<IBaseResult<List<UserPaymentMethod>>> ListByUserId(int userId)
        {
            try
            {
                var a = (IQueryable<UserPaymentMethod>) DataContext.GetDbSet<UserPaymentMethod>();
                a = a
                    .Where(entity => entity.Status > 0)
                    .Where(entity => entity.UserId == userId);

                var result = await a.ToListAsync();
                return BaseResult<List<UserPaymentMethod>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<UserPaymentMethod>>.FromError(e.ToString());
            }
        }
    }
}