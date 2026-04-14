using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Basket;

namespace Talabat.Core.Services.Content
{
    public interface IPaymentServices
    {
        Task<CustomerBasket?> CreateOrUPdatePaymentIntent(string BasketId);
         
        
        
    }
}
