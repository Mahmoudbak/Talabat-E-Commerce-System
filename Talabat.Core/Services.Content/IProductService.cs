using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Product;
using Talabat.Core.specifications.ProductSpec;

namespace Talabat.Core.Services.Content
{
    public interface    IProductService
    {
        Task<IReadOnlyList<Product>> GetProductAsync(ProductSprcParams SpecParams);

        Task<Product?> GetProductByIdAsync(int productId);

        Task<int>GetCountAsync(ProductSprcParams SpecParams);

        Task<IReadOnlyList<ProductBrand>>productBrandsAsync();
        
        Task<IReadOnlyList<ProductCategory>>productCategoriesAsync();   

    }
}
