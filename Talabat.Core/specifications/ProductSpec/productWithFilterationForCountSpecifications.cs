using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Product;
namespace Talabat.Core.specifications.ProductSpec
{
    public class productWithFilterationForCountSpecifications
     : BaseSpecifcations<Product>
    {
        public productWithFilterationForCountSpecifications(ProductSprcParams SpecParams)
             : base(p =>
                      (string.IsNullOrEmpty(SpecParams.Search)) || p.Name.ToLower().Contains(SpecParams.Search) &&
                      (!SpecParams.BrandId.HasValue || p.BrandId == SpecParams.BrandId.Value) &&
                      (!SpecParams.CategoryId.HasValue || p.CategoryId == SpecParams.CategoryId.Value)
            )
        {
        }
    }

}
