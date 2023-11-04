using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class ProductTypeRepository : BaseRepository<ProductType>
    {
        public ProductTypeRepository(DataContext dataContext) : base(dataContext) {}
    }
}