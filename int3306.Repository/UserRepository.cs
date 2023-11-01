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
            var db = dataContext.GetDbSet<User>();
            var result = db.FirstOrDefaultAsync(u => u.Username == username);
            return await result;
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

                var userDetails = new UserDetails();

                await dataContext.Database.BeginTransactionAsync();
                dataContext.Add(newUser);
                await dataContext.SaveChangesAsync();

                userDetails.Id = newUser.Id;
                dataContext.Add(userDetails);
                await dataContext.SaveChangesAsync();
                await dataContext.Database.CommitTransactionAsync();

                return BaseResult<bool>.FromSuccess(true);
            }
            catch (Exception e)
            {
                await dataContext.Database.RollbackTransactionAsync();
                return BaseResult<bool>.FromError(e.ToString());
            }
        }
    }
}