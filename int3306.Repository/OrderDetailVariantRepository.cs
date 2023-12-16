using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class OrderDetailVariantRepository : BaseRepository<OrderDetailVariant>
    {
        public OrderDetailVariantRepository(DataContext dataContext) : base(dataContext) {}
    }
}