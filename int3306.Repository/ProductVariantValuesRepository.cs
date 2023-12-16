using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class ProductVariantValuesRepository : BaseRepository<ProductVariantValue>
    {
        public ProductVariantValuesRepository(DataContext dataContext) : base(dataContext) {}
    }
}