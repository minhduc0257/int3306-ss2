using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class UserAddressRepository : BaseRepository<UserAddress>
    {
        public UserAddressRepository(DataContext dataContext) : base(dataContext) {}
    }
}