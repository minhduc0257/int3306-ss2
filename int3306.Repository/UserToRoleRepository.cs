using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class UserToRoleRepository : BaseRepository<UserToRole>
    {
        public UserToRoleRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<IBaseResult<List<UserToRole>>> ListByUserId(int userId)
        {
            try
            {
                var a = (IQueryable<UserToRole>) DataContext.GetDbSet<UserToRole>()
                    .Include(r => r.Role)
                    .Where(entity => entity.Status > 0)
                    .Where(entity => entity.UserId == userId);


                var result = await a.ToListAsync();
                return BaseResult<List<UserToRole>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<UserToRole>>.FromError(e.ToString());
            }
        }
        
        public async Task<IBaseResult<List<UserToRole>>> BulkAdd(int userId, List<int> roleId)
        {
            try
            {
                var db = DataContext.GetDbSet<UserToRole>();
                var entries = roleId.Select(t => new UserToRole
                {
                    RoleId = t,
                    UserId = userId
                }).ToList();
                await db.AddRangeAsync(entries);
                await DataContext.SaveChangesAsync();

                return BaseResult<List<UserToRole>>.FromSuccess(entries);
            }
            catch (Exception e)
            {
                return BaseResult<List<UserToRole>>.FromError(e.ToString());
            }
        }
    }
}