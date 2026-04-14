using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Order_Aggreate;

namespace Talabat.Core.specifications.OrderSpec
{
    public class OrderSpecifications:BaseSpecifcations<Order>
    {
        public OrderSpecifications(string buyerEmail):
            base(o=>o.BuyerEmail==buyerEmail) 
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o=>o.Items);

            AddOrderByDesc(o => o.OrderDate);   
        }

        public OrderSpecifications( int orderId,string buyerEmail) :
            base(O=>O.Id==orderId && O.BuyerEmail==buyerEmail ) 
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }

      
    }
}
