using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entity.Product
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public ProductBrand productBrand { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory productCategory { get; set; }//navgational prop to {one}

    }
}
