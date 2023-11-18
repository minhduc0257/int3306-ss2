using System.Reflection;
using int3306.Entities;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class DataContext : DbContext
    {
#pragma warning disable CS8618
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<T> GetDbSet<T>() where T : class
        {
            var r = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var property = r.FirstOrDefault(
                p =>
                {
                    var pt = p.PropertyType;
                    var isGeneric = pt.IsGenericType;
                    if (!isGeneric) return false;

                    var isDbSet = pt.GetGenericTypeDefinition() == typeof(DbSet<>);
                    if (!isDbSet) return false;

                    var genericArguments = pt.GetGenericArguments();
                    return genericArguments.Length == 1 && genericArguments[0] == typeof(T);
                }
            );

            var result = property?.GetValue(this) as DbSet<T>;
            return result ?? throw new NotImplementedException($"DbSet for type {typeof(T)} not implemented!");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<ProductToTag> ProductToTag { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToRole> UserToRole { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserDetail>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductToTag>()
                .HasOne(pt => pt.Product)
                .WithMany(pt => pt.ProductToTags)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<UserToRole>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserToRoles)
                .HasForeignKey(ur => ur.UserId);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        
#pragma warning restore CS8618
    }
}