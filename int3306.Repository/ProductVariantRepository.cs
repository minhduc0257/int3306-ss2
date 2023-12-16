using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class ProductVariantRepository : BaseRepository<ProductVariant>
    {
        public ProductVariantRepository(DataContext dataContext) : base(dataContext) {}
    }
}