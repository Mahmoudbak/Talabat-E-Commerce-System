using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity;
using Talabat.Core.Entity.Order_Aggreate;
using Talabat.Core.Entity.Product;
using Talabat.Core.Repository.content;

namespace Talabat.Core
{
    public interface IUnitOfWork:IAsyncDisposable 
    {
        //public IGenaricrepository<Product> productsRepo { get; set; }
        //public IGenaricrepository<ProductBrand> BrandsRepo { get; set; }
        //public IGenaricrepository<ProductCategory> categoriesRepo { get; set; }
        //public IGenaricrepository<DeliveryMethod> deliveryMethodsRepo { get; set; }
        //public IGenaricrepository<OrderItem> ordersItemsRepo { get; set; }
        //public IGenaricrepository<Order> ordersRepo { get; set; }

        IGenaricrepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        
        

        Task<int> CompleteAsync();
    }
}
