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
            serviceCollection.AddDbContext<DataContext>(optionsAction);

            serviceCollection.AddScoped<IBaseRepository<User>, UserRepository>();
            serviceCollection.AddScoped<IBaseRepository<ProductType>, ProductTypeRepository>();
            serviceCollection.AddScoped<IBaseRepository<ProductTag>, ProductTagRepository>();
            serviceCollection.AddScoped<IBaseRepository<ProductToTag>, ProductToTagRepository>();
            serviceCollection.AddScoped<IBaseRepository<UserDetail>, UserDetailRepository>();
            serviceCollection.AddScoped<IBaseRepository<Product>, ProductRepository>();
            serviceCollection.AddScoped<IBaseRepository<UserPaymentMethod>, UserPaymentMethodRepository>();
            serviceCollection.AddScoped<IBaseRepository<Stock>, StockRepository>();
            serviceCollection.AddScoped<IBaseRepository<UserAddress>, UserAddressRepository>();
            serviceCollection.AddScoped<IBaseRepository<Order>, OrderRepository>();
            serviceCollection.AddScoped<IBaseRepository<OrderDetail>, OrderDetailRepository>();
            serviceCollection.AddScoped<IBaseRepository<Cart>, CartRepository>();

            serviceCollection.AddScoped<UserRepository>();
            serviceCollection.AddScoped<ProductToTagRepository>();
            serviceCollection.AddScoped<ProductRepository>();
            serviceCollection.AddScoped<CartRepository>();
            return serviceCollection;
        }
    }
}