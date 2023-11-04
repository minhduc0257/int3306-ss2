using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace int3306.Repository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction)
        {
            serviceCollection.AddDbContextPool<DataContext>(optionsAction);
            
            serviceCollection.AddScoped<IBaseRepository<User>, UserRepository>();
            serviceCollection.AddScoped<IBaseRepository<ProductType>, ProductTypeRepository>();
            serviceCollection.AddScoped<IBaseRepository<UserDetail>, UserDetailRepository>();

            serviceCollection.AddScoped<UserRepository>();
            return serviceCollection;
        }
    }
}