using int3306.Entities.Shared;

namespace int3306.Repository.Shared
{
    public interface IBaseRepository<TEntity> where TEntity : IBaseEntity
    {
        public Task<IBaseResult<TEntity>> Get(int id);
        public Task<IBaseResult<int>> Post(TEntity entity);
        public Task<IBaseResult<TEntity>> Put(int id, TEntity entity);
        public Task<IBaseResult<TEntity>> Delete(int id);
    }
}