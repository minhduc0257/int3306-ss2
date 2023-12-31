using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(DataContext dataContext) : base(dataContext) {}
    }
}