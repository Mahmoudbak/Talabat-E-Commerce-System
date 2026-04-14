using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Error;
using Talabat.Core.Entity.Basket;
using Talabat.Core.Services.Content;

namespace Talabat.APIs.Controllers
{
     [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentServices _paymentServices;

        public PaymentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]

        [HttpPost("{basketid}")]// Get :api/payment/{basketid}
        public async Task<ActionResult<CustomerBasket>>CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket= await _paymentServices.CreateOrUPdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400,"An Error in Your Basket"));

            return Ok(basket);
        }
        
        //[HttpPost("webhook")]
        //public async Task<IActionResult> WebHook()
        //{ 
        //    var json= await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    try 
        //    {
        //        var stripeEvent = EventUtility.ConstructEvent(json,
        //            Request.Headers["Stripe-Signature"], endpointsecrect);

        //        Console.WriteLine("Unhandled event type {0}", stripeEvent.Type);

        //        return Ok();

        //    }
        //    catch(StripeException e) 
        //    {
        //        return BadRequest();
        //    }
        //}




    }
}
