using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class StockRepository : BaseRepository<Stock>
    {
        public StockRepository(DataContext dataContext) : base(dataContext) {}

        public override Task<IBaseResult<Stock>> Put(int id, Stock entity)
        {
            entity.LastModified = DateTime.Now;
            return base.Put(id, entity);
        }

        public override Task<IBaseResult<int>> Post(Stock entity)
        {
            entity.LastModified = DateTime.Now;
            return base.Post(entity);
        }


        public override async Task<IBaseResult<List<Stock>>> List(bool inUse = true)
        {
            try
            {
                var a = (IQueryable<Stock>) DataContext.GetDbSet<Stock>()
                    .Include(s => s.Product);

                if (inUse)
                {
                    a = a.Where(entity => entity.Status > 0);
                }

                var result = await a.ToListAsync();
                return BaseResult<List<Stock>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<Stock>>.FromError(e.ToString());
            }
        }

        public override async Task<IBaseResult<Stock>> Delete(int id)
        {
            try
            {
                var db = DataContext.GetDbSet<Stock>();
                var entry = await db.FirstOrDefaultAsync(e => e.Id == id);
                
                if (entry == null) return BaseResult<Stock>.FromNotFound();

                entry.Status = -1;
                entry.LastModified = DateTime.Now;
                await DataContext.SaveChangesAsync();
                return BaseResult<Stock>.FromSuccess(null);

            }
            catch (Exception e)
            {
                return BaseResult<Stock>.FromError(e.ToString());
            }
        }
    }
}