using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(DataContext dataContext) : base(dataContext) {}

        public override async Task<IBaseResult<int>> Post(Order entity)
        {
            try
            {
                await DataContext.Database.BeginTransactionAsync();
                var orderDb = DataContext.GetDbSet<Order>();
                var orderDetailDb = DataContext.GetDbSet<OrderDetail>();
                entity.Id = 0;
                entity.Status = 1;
                var entry = await orderDb.AddAsync(entity);
                await DataContext.SaveChangesAsync();

                foreach (var detail in entity.OrderDetails)
                {
                    detail.OrderId = entity.Id;
                }
                await orderDetailDb.AddRangeAsync(entity.OrderDetails);

                await DataContext.Database.CommitTransactionAsync();

                return BaseResult<int>.FromSuccess(entity.Id);
            }
            catch (Exception e)
            {
                await DataContext.Database.RollbackTransactionAsync();
                return BaseResult<int>.FromError(e.ToString());
            }
        }
    }
}