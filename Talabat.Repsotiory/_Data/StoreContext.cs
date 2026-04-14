using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Talabat.Core.Entity.Employee;
using Talabat.Core.Entity.Employees.Employee;
using Talabat.Core.Entity.Order_Aggreate;
using Talabat.Core.Entity.Product;
namespace Talabat.Repsotiory.Data
{
    public class StoreContext:DbContext
    {
        public StoreContext(DbContextOptions<StoreContext>options)
            :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductBrandConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductCategoryConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //Chages from Assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductBrand>productBrands { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<Employee>Employees { get; set; }
        public DbSet<Depaetment>Depaetments { get; set; }
        public DbSet<Order>Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }



    }
}
