using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entity.Product;
using Talabat.Core.Services.Content;
using Talabat.Core.specifications.Product_Spec;
using Talabat.Core.specifications.ProductSpec;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Talabat.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public  async Task<IReadOnlyList<Product>> GetProductAsync(ProductSprcParams SpecParams)
        {
            var spec = new ProductWithBranAndCategory_Spec(SpecParams);

            var product = await _unitOfWork.Repository<Product>().GetAllwithSpecAsync(spec);

            return product;

        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var spec = new ProductWithBranAndCategory_Spec(productId);
            var product = await _unitOfWork.Repository<Product>().GetByIdwithSpecAsync(spec);

            return product;
        }
        public Task<int> GetCountAsync(ProductSprcParams SpecParams)
        {

            var countSpec = new productWithFilterationForCountSpecifications(SpecParams);

            var count = _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
            return count;
        }

        public async Task<IReadOnlyList<ProductBrand>> productBrandsAsync()
           => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
        


          

        public async Task<IReadOnlyList<ProductCategory>> productCategoriesAsync()
             => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
        

    }
}
