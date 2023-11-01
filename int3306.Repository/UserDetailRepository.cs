using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class UserDetailRepository : BaseRepository<UserDetail>
    {
        public UserDetailRepository(DataContext dataContext) : base(dataContext) {}
    }
}