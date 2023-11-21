using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(DataContext dataContext) : base(dataContext) {}
    }
}