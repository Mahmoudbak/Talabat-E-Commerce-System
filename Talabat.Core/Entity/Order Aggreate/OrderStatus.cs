using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entity.Order_Aggreate
{
    public enum OrderStatus
    {
        //[EnumMember(Value ="Pending")]
        pending,
        PaymantRecevied,
        paymantFailed
    }
}
