using System.Net;

namespace int3306.Repository.Shared
{
    public interface IBaseResult<TEntity>
    {
        public TEntity? Data { get; init; }
        public bool Success { get; init; }
        public string Error { get; init; }
        
        public HttpStatusCode? StatusCodeHint { get; init; }
    }
}