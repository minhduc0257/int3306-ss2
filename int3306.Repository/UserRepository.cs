using System.Net;
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

        public override Task<IBaseResult<User>> Get(int id)
        {
            return GetWithDetail(id);
        }

        public async Task<IBaseResult<User>> GetWithDetail(int id)
        {
            var db = DataContext.GetDbSet<User>();
            var result = await db
                .Include(u => u.Detail)
                .Include(u => u.UserAddress)
                .Include(u => u.UserToRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (result == null)
            {
                return BaseResult<User>.FromNotFound();
            }

#pragma warning disable CS8619
            result.Roles = (List<Role>) result.UserToRoles.Select(r => r.Role).Where(r => r != null).ToList();
            result.Password = "";
#pragma warning restore CS8619
            
            return BaseResult<User>.FromSuccess(result);
        }

        public override async Task<IBaseResult<List<User>>> List(bool inUse = true)
        {
            try
            {
                var a = (IQueryable<User>)DataContext.GetDbSet<User>()
                    .Include(u => u.Detail)
                    .Include(u => u.UserAddress)
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
                    r.Password = "";
                }
                
                return BaseResult<List<User>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<User>>.FromError(e.ToString());
            }
        }

        public async Task<IBaseResult<bool>> Register(string username, string hashedPassword, string name, string email)
        {
            try
            {
                var newUser = new User
                {
                    CreationTime = DateTimeOffset.Now,
                    Id = 0,
                    Name = string.IsNullOrWhiteSpace(name) ? username : name,
                    Username = username,
                    Password = hashedPassword
                };

                var userDetails = new UserDetail
                {
                    Email = email
                };

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

        public async Task<IBaseResult<bool>> ChangePassword(int userId, string old, string @new)
        {
            
            try
            {
                var p = BC.HashPassword(old);
                var r = await DataContext.GetDbSet<User>().AnyAsync(u => u.Id == userId);

                if (!r)
                {
                    return BaseResult<bool>.FromNotFound();
                }

                await DataContext.Database.BeginTransactionAsync();
                var record = await DataContext.GetDbSet<User>().FirstAsync(u => u.Id == userId);
                if (!BC.Verify(old, record.Password))
                {
                    await DataContext.Database.RollbackTransactionAsync();
                    return BaseResult<bool>.FromError("Wrong password", HttpStatusCode.Forbidden);
                }

                record.Password = BC.HashPassword(@new);
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