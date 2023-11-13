using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class UserPaymentMethodRepository : BaseRepository<Entities.UserPaymentMethod>
    {
        public UserPaymentMethodRepository(DataContext dataContext) : base(dataContext) {}
    }
}