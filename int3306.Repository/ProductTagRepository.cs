using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class ProductTagRepository : BaseRepository<ProductTag>
    {
        public ProductTagRepository(DataContext dataContext) : base(dataContext) {}
    }
}