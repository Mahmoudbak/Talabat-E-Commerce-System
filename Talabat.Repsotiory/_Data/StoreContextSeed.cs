using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entity.Order_Aggreate;
using Talabat.Core.Entity.Product;

namespace Talabat.Repsotiory.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbContext)
        {
            #region DataSeed in productBrand
            if (_dbContext.productBrands.Count() == 0)
            {

                var brandData = File.ReadAllText("../Talabat.Repsotiory/Data/DataSeed/brands.json");
                var brand = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                #region if the ID Identity
                //brand = brand.Select(b => new ProductBrand()
                //{
                //    Name = b.Name,
                //    Id = b.Id
                //}); 
                #endregion
                if (brand?.Count() > 0)
                {
                    foreach (var brands in brand)
                    {
                        _dbContext.Set<ProductBrand>().Add(brands);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            #endregion
            #region DataSeed of ProductCategory
            if (_dbContext.productCategories.Count() == 0)
            {
                //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                // var caregoryData = File.ReadAllText("../Talabat.Repsotiory/Data/DataSeed/categories.json");
                var caregoryData = File.ReadAllText("../Talabat.Repsotiory/Data/DataSeed/categories.json");

                var category = JsonSerializer.Deserialize<List<ProductCategory>>(caregoryData);
                if (category?.Count() > 0)
                    foreach (var cate in category)
                    {
                        _dbContext.Set<ProductCategory>().Add(cate);
                    }
                await _dbContext.SaveChangesAsync();
            }
            #endregion
            #region Dataseed of Product
            if (_dbContext.products.Count() == 0)
            {
                var ProductData = File.ReadAllText("../Talabat.Repsotiory/Data/DataSeed/products.json");
                var product = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (product?.Count > 0)
                {
                    foreach (var prod in product)
                    {
                        _dbContext.Set<Product>().Add(prod);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            #endregion


            #region DataSeed in Order
            if (!_dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repsotiory/Data/DataSeed/delivery.json");
                var DeliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                #region if the ID Identity
                //brand = brand.Select(b => new ProductBrand()
                //{
                //    Name = b.Name,
                //    Id = b.Id
                //}); 
                #endregion
                if (DeliveryMethod?.Count() > 0)
                {
                    foreach (var Delivery in DeliveryMethod)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(Delivery);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            //if (!context.DeliveryMethods.Any())
            //{

            //    var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/SeedData/delivery.json");
            //    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

            //    foreach (var deliveryMethod in deliveryMethods)
            //        context.Set<DeliveryMethod>().Add(deliveryMethod);

            //    await context.SaveChangesAsync();
            //}
            #endregion
        }
    }
}
