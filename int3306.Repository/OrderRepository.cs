using int3306.Entities;
using int3306.Entities.Shared;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(DataContext dataContext) : base(dataContext) {}

        private IQueryable<Order> GetBaseJoinedQuery()
        {
            return DataContext.GetDbSet<Order>()
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .ThenInclude(d => d.ProductThumbnails)

                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.OrderDetailVariant)
                .ThenInclude(v => v.Variant)

                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.OrderDetailVariant)
                .ThenInclude(v => v.VariantValue)
                .Include(o => o.User)
                .AsSplitQuery();
        }
        
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
                    c.Status = -1;
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

        public override async Task<IBaseResult<Order>> Put(int id, Order entity)
        {
            try
            {
                var current = await DataContext.GetDbSet<Order>().FirstOrDefaultAsync(e => e.Id == id);
                
                if (current == null) return BaseResult<Order>.FromNotFound();
                var entry = DataContext.Entry(current);

                entity.Id = id;
                if (current.Status != entity.Status)
                {
                    switch ((OrderStatusIndex) entity.Status)
                    {
                        case OrderStatusIndex.Cancelled: 
                            entity.TimeCancelled = DateTime.Now;
                            break;
                        case OrderStatusIndex.Preparing: 
                            entity.TimePreparing = DateTime.Now;
                            break;
                        case OrderStatusIndex.Shipping: 
                            entity.TimeShipping = DateTime.Now;
                            break;
                        case OrderStatusIndex.Shipped: 
                            entity.TimeShipped = DateTime.Now;
                            break;
                    }
                }
                
                entry.CurrentValues.SetValues(entity);
                await DataContext.SaveChangesAsync();
                return BaseResult<Order>.FromSuccess(entry.Entity);
            }
            catch (Exception e)
            {
                return BaseResult<Order>.FromError(e.ToString());
            }
        }

        public async Task<IBaseResult<List<Order>>> ListByUserId(int userId)
        {
            try
            {
                var a = GetBaseJoinedQuery()
                    .Where(entity => entity.Status > 0)
                    .Where(entity => entity.UserId == userId)
                    .OrderByDescending(entity => entity.Timestamp);

                var result = await a.ToListAsync();
                foreach (var o in result)
                {
                    if (o.User != null)
                    {
                        o.User.Password = "";
                    }
                }
                return BaseResult<List<Order>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<Order>>.FromError(e.ToString());
            }
        }

        public override async Task<IBaseResult<List<Order>>> List(bool inUse = true)
        {
            try
            {
                var a = GetBaseJoinedQuery();

                if (inUse)
                {
                    a = a.Where(entity => entity.Status > 0);
                }

                a = a.OrderByDescending(entity => entity.Timestamp);

                var result = await a.ToListAsync();
                foreach (var o in result)
                {
                    if (o.User != null)
                    {
                        o.User.Password = "";
                    }
                }
                return BaseResult<List<Order>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<Order>>.FromError(e.ToString());
            }
        }

        public override async Task<IBaseResult<Order>> Get(int id)
        {
            try
            {
                var r = await GetBaseJoinedQuery()
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (r?.User != null)
                {
                    r.User.Password = "";
                }
                return r != null ? BaseResult<Order>.FromSuccess(r) : BaseResult<Order>.FromNotFound();
            }
            catch (Exception e)
            {
                return BaseResult<Order>.FromError(e.ToString());
            }
        }
    }
}