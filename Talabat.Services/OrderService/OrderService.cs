using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entity.Order_Aggreate;
using Talabat.Core.Entity.Product;
using Talabat.Core.Repository.content;
using Talabat.Core.Repository.contrent;
using Talabat.Core.Services.Content;
using Talabat.Core.specifications.OrderSpec;
using Talabat.Core.specifications.ProductSpec;

namespace Talabat.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentServices _paymentServices;

        ///private readonly IGenaricrepository<Product> _ProductRepo;
        ///private readonly IGenaricrepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenaricrepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork,
            IPaymentServices paymentServices

            )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentServices = paymentServices;
            /// _ProductRepo = ProductRepo;
            /// _deliveryMethodRepo = deliveryMethodRepo;
            /// _orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address shippingAddress)
        {
            // 1.Get Basket From Baskets Repo

            var basket = await _basketRepo.GetBasketAsync(basketId);


            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0) 
            {
                var ProductRepository = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await ProductRepository.GetByIdAsync(item.Id);


                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }

            }


            // 3. Calculate SubTotal
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo
          
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliverMethodId);

            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderWithPaymentIntentSpecification(basket?.PaymentIntentId);

            var existingOrder= await orderRepo.GetByIdwithSpecAsync(spec);

            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);
                await _paymentServices.CreateOrUPdatePaymentIntent(basketId);
                
            }

            // 5. Create Order
            var order= new Order(
                    buyerEmail: buyerEmail,
                    shippingAddress: shippingAddress,
                    deliveryMethod: deliveryMethod,
                    items: orderItems,
                    subTotal: subtotal,
                    paymantIntentId:basket?.PaymentIntentId ?? ""
            );

            await _unitOfWork.Repository<Order>().AddAsync(order);

            // 6. Save To Database [TODO]
           var result=  await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        
        }
        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            var orderRepo=  _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var orders = await orderRepo.GetAllwithSpecAsync(spec);

            return orders;
        
        }

        public  Task<Order?> CreateOrderByIdForUserAsync(int orderId,string buyerEmail)
        {
            var orderRepo=_unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications( orderId ,buyerEmail );


            var orders = orderRepo.GetByIdwithSpecAsync(spec);


            return orders;


        }

            public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
                => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

     
    }
}
