using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entity.Identity;

namespace Talabat.APIs.Dtos
{
    public class OrderDTO
    {
        //[Required]
       // public string BuyerEmail { get; set; }
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }//DeliveryMethodId

        public AddressDto ShippingAddress { get; set; }



    }
}
