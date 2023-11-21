using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>
    {
        public OrderDetailRepository(DataContext dataContext) : base(dataContext) {}
    }
}