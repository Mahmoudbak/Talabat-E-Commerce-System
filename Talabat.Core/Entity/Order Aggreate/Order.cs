using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entity.Order_Aggreate
{
    public class Order:BaseEntity
    {
        // there is must by parameterless constructor Exist For EF Core 
        private Order()
        {

        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymantIntentId )
        {
            if (deliveryMethod == null)
            {
                throw new Exception("🛑 كارثة: الـ DeliveryMethod وصل هنا بـ NULL!");
            }
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymantIntentId = paymantIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; }=OrderStatus.pending;
        public Address ShippingAddress { get; set; } = null!;


        //public int DeliveryMethodId { get; set; }// Foreign Key
        public DeliveryMethod ?DeliveryMethod { get; set; } = null!;  //Navigational Property[One]

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); //Navigational Property[Many]


        public decimal SubTotal { get; set; }

        #region to Ways of Deliver Attrubat 
        //1.User Not Mapper & =>
        //[NotMapped]
        //public decimal Total => SubTotal + DeliveryMethod.Cost;

        //2.user Get
        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;
        //public decimal GetTotal()
        //{
        //    return SubTotal + (DeliveryMethod?.Cost ?? 0);
        //}

        #endregion

        public string PaymantIntentId { get; set; } 




    }
}
