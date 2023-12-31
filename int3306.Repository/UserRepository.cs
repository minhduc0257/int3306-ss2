using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace int3306.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<User?> GetByUsername(string username)
        {
            var db = DataContext.GetDbSet<User>();
            var result = db.FirstOrDefaultAsync(u => u.Username == username);
            return await result;
        }

        public async Task<IBaseResult<User>> GetWithDetail(int id)
        {
            var db = DataContext.GetDbSet<User>();
            var result = await db
                .Include(u => u.Detail)
                .Include(u => u.UserToRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (result == null)
            {
                return BaseResult<User>.FromNotFound();
            }

#pragma warning disable CS8619
            result.Roles = (List<Role>) result.UserToRoles.Select(r => r.Role).Where(r => r != null).ToList();
#pragma warning restore CS8619
            
            return BaseResult<User>.FromSuccess(result);
        }

        public override async Task<IBaseResult<List<User>>> List(bool inUse = true)
        {
            try
            {
                var a = (IQueryable<User>)DataContext.GetDbSet<User>()
                    .Include(u => u.Detail)
                    .Include(u => u.UserToRoles)
                    .ThenInclude(ur => ur.Role);

                if (inUse)
                {
                    a = a.Where(entity => entity.Status > 0);
                }

                var result = await a.ToListAsync();

                foreach (var r in result)
                {
                    r.Roles = r.UserToRoles.Select(r => r.Role!).Where(r => r != null).ToList();
                }
                
                return BaseResult<List<User>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<User>>.FromError(e.ToString());
            }
        }

        public async Task<IBaseResult<bool>> Register(string username, string hashedPassword)
        {
            try
            {
                var newUser = new User
                {
                    CreationTime = DateTimeOffset.Now,
                    Id = 0,
                    Name = username,
                    Username = username,
                    Password = hashedPassword
                };

                var userDetails = new UserDetail();

                await DataContext.Database.BeginTransactionAsync();
                DataContext.Add(newUser);
                await DataContext.SaveChangesAsync();

                userDetails.UserId = newUser.Id;
                DataContext.Add(userDetails);
                await DataContext.SaveChangesAsync();
                await DataContext.Database.CommitTransactionAsync();

                return BaseResult<bool>.FromSuccess(true);
            }
            catch (Exception e)
            {
                await DataContext.Database.RollbackTransactionAsync();
                return BaseResult<bool>.FromError(e.ToString());
            }
        }
    }
}