using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Product;
using Talabat.Core.specifications.ProductSpec;

namespace Talabat.Core.specifications.Product_Spec
{
  
        
    public class ProductWithBranAndCategory_Spec :BaseSpecifcations<Product> 
    {
    //This constructor will be using for creating an object ,that will be used Get All product  
        public  ProductWithBranAndCategory_Spec(ProductSprcParams SpecParams)
            :base(p=>
                      (string.IsNullOrEmpty(SpecParams.Search))||p.Name.ToLower().Contains(SpecParams.Search)&&
                      (!SpecParams.BrandId.HasValue||p.BrandId== SpecParams.BrandId.Value)&&
                      (!SpecParams.CategoryId.HasValue||p.CategoryId== SpecParams.CategoryId.Value)
            )
        {
            includes();
            if (!string.IsNullOrEmpty(SpecParams.Sort))
            {
                switch (SpecParams.Sort)
                {
                    case "priceAsc":
                        //OrderBy=p=>p.Price;
                        AddOrderBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        //OrderByDesc=P=>P.Price;
                        AddOrderByDesc(p=>p.Price);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;

                }
            }
            else
                AddOrderBy(p=>p.Name);
            //TotalProduct= 18 ~ 20
            //PageSize    =5
            //PageIndex   =3
            //query=_dbcontext.Products.Where(p=>true&&true).orderby(p=>p.Name).Skip(5).Take(5)
            ApplyPagination((SpecParams.PageIndex-1)*SpecParams.PageSize,SpecParams.PageSize);
            
        }
     
        //This constructor will be using for creating an object ,that will be used Get by Id product  
        public ProductWithBranAndCategory_Spec( int id):base(p=>p.Id==id)
        {
            includes(); 
        }
        private void includes()
        {
            Includes.Add(p => p.productBrand);
            Includes.Add(p => p.productCategory);
        }
    }
}
