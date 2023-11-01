using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace int3306.Repository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction)
        {
            serviceCollection.AddDbContextPool<DataContext>(optionsAction);
            serviceCollection.AddScoped<UserRepository>();
            serviceCollection.AddScoped<UserDetailRepository>();
            return serviceCollection;
        }
    }
}