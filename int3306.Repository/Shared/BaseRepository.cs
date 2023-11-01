using int3306.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository.Shared
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly DataContext dataContext;
        public BaseRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public virtual async Task<IBaseResult<TEntity>> Get(int id)
        {
            try
            {
                var r = await dataContext.GetDbSet<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
                return r != null ? BaseResult<TEntity>.FromSuccess(r) : BaseResult<TEntity>.FromNotFound();
            }
            catch (Exception e)
            {
                return BaseResult<TEntity>.FromError(e.ToString());
            }
        }

        public virtual async Task<IBaseResult<int>> Post(TEntity entity)
        {
            try
            {
                var db = dataContext.GetDbSet<TEntity>();
                entity.Id = 0;
                var entry = await db.AddAsync(entity);
                await dataContext.SaveChangesAsync();

                return BaseResult<int>.FromSuccess(entry.Entity.Id);
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
                var current = await dataContext.GetDbSet<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
                
                if (current == null) return BaseResult<TEntity>.FromNotFound();
                var entry = dataContext.Entry(current);
                entry.CurrentValues.SetValues(entity);
                await dataContext.SaveChangesAsync();
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
                var db = dataContext.GetDbSet<TEntity>();
                var entry = await db.FirstOrDefaultAsync(e => e.Id == id);
                
                if (entry == null) return BaseResult<TEntity>.FromNotFound();
                
                db.Remove(entry);
                await dataContext.SaveChangesAsync();
                return BaseResult<TEntity>.FromSuccess(null);

            }
            catch (Exception e)
            {
                return BaseResult<TEntity>.FromError(e.ToString());
            }
        }
    }
}