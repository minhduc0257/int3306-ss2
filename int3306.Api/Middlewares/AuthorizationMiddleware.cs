namespace int3306.Api.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate next;
        public AuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            // check for permission index here
            await next(ctx);
        }
    }
}