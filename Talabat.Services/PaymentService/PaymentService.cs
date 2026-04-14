using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entity.Basket;
using Talabat.Core.Entity.Order_Aggreate;
using Talabat.Core.Entity.Product;
using Talabat.Core.Repository.contrent;
using Talabat.Core.Services.Content;
using Product = Talabat.Core.Entity.Product.Product;



namespace Talabat.Services.PaymentService
{
    public class PaymentService : IPaymentServices
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUPdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepo.GetBasketAsync(BasketId);

            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue) 
            {
                var deliverymethod= await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                shippingPrice = deliverymethod.Cost;
                basket.ShippingPrice= shippingPrice;


            }

            if (basket.Items.Count > 0)
            {
                var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product =await productRepo.GetByIdAsync(item.Id);
                    if(item.Price!=product.Price)
                        item.Price = product.Price;


                }

            }

            PaymentIntent paymentIntent;

            PaymentIntentService paymentIntentService=new PaymentIntentService();



            if (string.IsNullOrEmpty(basket.PaymentIntentId))   //create New Payment Entent
            {
                var option = new PaymentIntentCreateOptions()
                {
                    Amount =(long) basket.Items.Sum(item => item.Price*100 * item.Quantity) +(long) shippingPrice*100, // times*100 علشان احول من قرش الي جنيه او من سينت الي دولار
                    Currency="usd",
                    PaymentMethodTypes=new List<string>() {"card"}
                };  
                paymentIntent =await paymentIntentService.CreateAsync(option); //Integration With Stripe

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;


            }
            else //update Exisiting Payment Intent 
            {

                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100, // times*100 علشان احول من قرش الي جنيه او من سينت الي دولار

                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options); 


            }

            await _basketRepo.UpdateBasketAsync(basket);

            return basket;
        }
    }
}
