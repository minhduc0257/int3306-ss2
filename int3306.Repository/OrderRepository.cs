using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(DataContext dataContext) : base(dataContext) {}

        public override async Task<IBaseResult<int>> Post(Order entity)
        {
            try
            {
                var cartDb = DataContext.GetDbSet<Cart>();
                var carts = await cartDb
                    .Include(c => c.Product)
                    .Where(cart => entity.CartId!.Contains(cart.Id))
                    .ToListAsync();

                await DataContext.Database.BeginTransactionAsync();
                var orderDb = DataContext.GetDbSet<Order>();
                var orderDetailDb = DataContext.GetDbSet<OrderDetail>();
                entity.Timestamp = DateTime.Now;
                entity.Id = 0;
                entity.Status = 1;
                var entry = await orderDb.AddAsync(entity);
                await DataContext.SaveChangesAsync();

                var orderDetails = carts.Select(c =>
                {
                    var price = (c.Product?.Price ?? 0) * c.Count;
                    return new OrderDetail
                    {
                        Count = c.Count,
                        OrderId = entity.Id,
                        ProductId = c.ProductId,
                        TotalPrice = price
                    };
                }).ToList();
                entity.TotalPrice = orderDetails.Select(d => d.TotalPrice).Sum();
                    
                await orderDetailDb.AddRangeAsync(orderDetails);
                await DataContext.SaveChangesAsync();

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