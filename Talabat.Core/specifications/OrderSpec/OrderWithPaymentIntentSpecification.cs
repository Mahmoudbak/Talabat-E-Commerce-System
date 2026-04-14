using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Order_Aggreate;

namespace Talabat.Core.specifications.OrderSpec
{
    public class OrderWithPaymentIntentSpecification:BaseSpecifcations<Order>
    {
        public OrderWithPaymentIntentSpecification(string? paymentIntentId)
            :base(O=>O.PaymantIntentId==paymentIntentId)
        {
            
        }
    }
}
