using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entity.Basket;
using Talabat.Core.Entity.Order_Aggreate;

//using Talabat.Core.Entity.Identity;
using Talabat.Core.Entity.Product;
using static System.Net.WebRequestMethods;

namespace Talabat.APIs.Helper
{
    public class MappingProfile:Profile
    {


        public MappingProfile()
        {


            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.productBrand, o => o.MapFrom(s => s.productBrand.Name))
                .ForMember(d => d.productCategory, o => o.MapFrom(s => s.productCategory.Name))
                    //That is not correct to localhost not constant
                    //that not correct 
                    //.ForMember(d => d.PictureUrl, o => o.MapFrom(s => $"{"https://localhost:7170"}/{s.PictureUrl}"));
                    .ForMember(d => d.PictureUrl, o => o.MapFrom < ProductPictureUrlResolver>());

            CreateMap< AddressDto,Core.Entity.Identity.Address>().ReverseMap();
            //CreateMap<CustomerBasketDto>


            CreateMap<AddressDto, Address>();
                
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d=>d.DeliveryMethodCost,o=>o.MapFrom(s=>s.DeliveryMethod.Cost));


            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d=>d.ProductId,o=>o.MapFrom(s=>s.Product.ProductId))
                .ForMember(d=>d.ProductName,o=>o.MapFrom(s=>s.Product.ProductName))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom(s=>s.Product.PictureUrl))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderItemPictureUrlResolver>() );

            CreateMap<CustomerBasketDto, CustomerBasket>();

        
        }
    }
}
