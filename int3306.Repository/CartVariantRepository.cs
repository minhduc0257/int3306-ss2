using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class CartVariantRepository : BaseRepository<CartVariant>
    {
        public CartVariantRepository(DataContext dataContext) : base(dataContext) {}
    }
}