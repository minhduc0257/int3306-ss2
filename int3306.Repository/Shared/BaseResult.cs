namespace int3306.Repository.Shared
{
    public class BaseResult<TEntity> : IBaseResult<TEntity>
    {
        public TEntity? Data { get; init; }
        public bool Success { get; init; }
        public string Error { get; init; } = "";

        public static BaseResult<TEntity> FromSuccess(TEntity? data)
        {
            return new BaseResult<TEntity>
            {
                Data = data,
                Success = true,
                Error = ""
            };
        }
        
        public static BaseResult<TEntity> FromNotFound()
        {
            return new BaseResult<TEntity>
            {
                Data = default,
                Success = false,
                Error = "NotFound"
            };
        }
        
        public static BaseResult<TEntity> FromError(string error)
        {
            return new BaseResult<TEntity>
            {
                Data = default,
                Success = false,
                Error = error
            };
        }
    }
}