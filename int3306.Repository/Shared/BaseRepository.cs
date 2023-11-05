using int3306.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository.Shared
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly DataContext DataContext;
        protected BaseRepository(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public virtual async Task<IBaseResult<TEntity>> Get(int id)
        {
            try
            {
                var r = await DataContext.GetDbSet<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
                return r != null ? BaseResult<TEntity>.FromSuccess(r) : BaseResult<TEntity>.FromNotFound();
            }
            catch (Exception e)
            {
                return BaseResult<TEntity>.FromError(e.ToString());
            }
        }
        
        public virtual async Task<IBaseResult<List<TEntity>>> List(bool inUse = true)
        {
            try
            {
                var a = (IQueryable<TEntity>) DataContext.GetDbSet<TEntity>();

                if (inUse)
                {
                    a = a.Where(entity => entity.Status > 0);
                }

                var result = await a.ToListAsync();
                return BaseResult<List<TEntity>>.FromSuccess(result);
            }
            catch (Exception e)
            {
                return BaseResult<List<TEntity>>.FromError(e.ToString());
            }
        }

        public virtual async Task<IBaseResult<int>> Post(TEntity entity)
        {
            try
            {
                var db = DataContext.GetDbSet<TEntity>();
                entity.Id = 0;
                entity.Status = 1;
                var entry = await db.AddAsync(entity);
                await DataContext.SaveChangesAsync();

                return BaseResult<int>.FromSuccess(entity.Id);
            }
            catch (Exception e)
            {
                return BaseResult<int>.FromError(e.ToString());
            }
        }

        public virtual async Task<IBaseResult<TEntity>> Put(int id, TEntity entity)
        {
            try
            {
                var current = await DataContext.GetDbSet<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
                
                if (current == null) return BaseResult<TEntity>.FromNotFound();
                var entry = DataContext.Entry(current);

                entity.Id = id;
                entry.CurrentValues.SetValues(entity);
                await DataContext.SaveChangesAsync();
                return BaseResult<TEntity>.FromSuccess(entry.Entity);
            }
            catch (Exception e)
            {
                return BaseResult<TEntity>.FromError(e.ToString());
            }
        }

        public virtual async Task<IBaseResult<TEntity>> Delete(int id)
        {
            try
            {
                var db = DataContext.GetDbSet<TEntity>();
                var entry = await db.FirstOrDefaultAsync(e => e.Id == id);
                
                if (entry == null) return BaseResult<TEntity>.FromNotFound();

                entry.Status = -1;
                await DataContext.SaveChangesAsync();
                return BaseResult<TEntity>.FromSuccess(null);

            }
            catch (Exception e)
            {
                return BaseResult<TEntity>.FromError(e.ToString());
            }
        }

        public virtual async Task<IBaseResult<int>> BulkDelete(List<int> id)
        {
            try
            {
                var db = DataContext.GetDbSet<TEntity>();
                var data = await db.Where(e => id.Contains(e.Id)).ToListAsync();
                await DataContext.Database.BeginTransactionAsync();

                foreach (var e in data)
                {
                    e.Status = -1;
                }

                await DataContext.SaveChangesAsync();
                await DataContext.Database.CommitTransactionAsync();
                return BaseResult<int>.FromSuccess(data.Count);
            }
            catch (Exception e)
            {
                await DataContext.Database.RollbackTransactionAsync();
                return BaseResult<int>.FromError(e.ToString());
            }
        }
    }
}