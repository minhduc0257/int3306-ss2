using System.Net;
using System.Text.Json.Serialization;

namespace int3306.Repository.Shared
{
    public class BaseResult<TEntity> : IBaseResult<TEntity>
    {
        public TEntity? Data { get; init; }
        public bool Success { get; init; }
        public string Error { get; init; } = "";
        
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public HttpStatusCode? StatusCodeHint { get; init; }

        public static BaseResult<TEntity> FromSuccess(TEntity? data)
        {
            return new BaseResult<TEntity>
            {
                Data = data,
                Success = true,
                Error = "",
                StatusCodeHint = HttpStatusCode.OK
            };
        }
        
        public static BaseResult<TEntity> FromNotFound()
        {
            return new BaseResult<TEntity>
            {
                Data = default,
                Success = false,
                Error = "NotFound",
                StatusCodeHint = HttpStatusCode.NotFound
            };
        }
        
        public static BaseResult<TEntity> FromError(string error, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            return new BaseResult<TEntity>
            {
                Data = default,
                Success = false,
                Error = error,
                StatusCodeHint = code
            };
        }
        
        public static BaseResult<TNewEntity> IntoError<TNewEntity>(IBaseResult<TEntity> entity)
        {
            return new BaseResult<TNewEntity>
            {
                Data = default,
                Success = false,
                Error = entity.Error,
                StatusCodeHint = entity.StatusCodeHint
            };
        }
    }
}