using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class UserAddressRepository : BaseRepository<UserAddress>
    {
        public UserAddressRepository(DataContext dataContext) : base(dataContext) {}
        
        public async Task<IBaseResult<List<UserAddress>>> ListByUserId(int userId)
        {
            try
            {
                var a = (IQueryable<UserAddress>) DataContext.GetDbSet<UserAddress>();
                a = a
                    .Where(entity => entity.Status > 0)
                    .Where(entity => entity.UserId == userId);

                var result = await a.ToListAsync();
                return BaseResult<List<UserAddress>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<UserAddress>>.FromError(e.ToString());
            }
        }
    }
}