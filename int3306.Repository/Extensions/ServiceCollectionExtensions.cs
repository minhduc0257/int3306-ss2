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
            serviceCollection.AddScoped<IBaseRepository<ProductThumbnail>, ProductThumbnailRepository>();
            serviceCollection.AddScoped<IBaseRepository<ProductVariant>, ProductVariantRepository>();
            serviceCollection.AddScoped<IBaseRepository<ProductVariantValue>, ProductVariantValuesRepository>();
            serviceCollection.AddScoped<IBaseRepository<OrderDetailVariant>, OrderDetailVariantRepository>();
            serviceCollection.AddScoped<IBaseRepository<CartVariant>, CartVariantRepository>();
            serviceCollection.AddScoped<IBaseRepository<Asset>, AssetRepository>();
            serviceCollection.AddScoped<IBaseRepository<UserToRole>, UserToRoleRepository>();
            serviceCollection.AddScoped<IBaseRepository<Role>, RoleRepository>();


            serviceCollection.AddScoped<UserRepository>();
            serviceCollection.AddScoped<ProductTypeRepository>();
            serviceCollection.AddScoped<ProductTagRepository>();
            serviceCollection.AddScoped<ProductToTagRepository>();
            serviceCollection.AddScoped<UserDetailRepository>();
            serviceCollection.AddScoped<ProductRepository>();
            serviceCollection.AddScoped<UserPaymentMethodRepository>();
            serviceCollection.AddScoped<StockRepository>();
            serviceCollection.AddScoped<UserAddressRepository>();
            serviceCollection.AddScoped<OrderRepository>();
            serviceCollection.AddScoped<OrderDetailRepository>();
            serviceCollection.AddScoped<CartRepository>();
            serviceCollection.AddScoped<ProductThumbnailRepository>();
            serviceCollection.AddScoped<ProductVariantRepository>();
            serviceCollection.AddScoped<ProductVariantValuesRepository>();
            serviceCollection.AddScoped<CartVariantRepository>();
            serviceCollection.AddScoped<AssetRepository>();
            serviceCollection.AddScoped<UserToRoleRepository>();
            serviceCollection.AddScoped<RoleRepository>();
            return serviceCollection;
        }
    }
}
