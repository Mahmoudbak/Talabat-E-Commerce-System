using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Error;
using Talabat.Repsotiory.Data;

namespace Talabat.APIs.Controllers
{
    // The Controller is Front End Dev or Mobile 
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbContext)
        {
            _dbcontext = dbContext;
        }
        [HttpGet("NotFound")] // Get :  Api/bugge/NotFound
        public ActionResult GetNotFoundRequest()
        {
            var Product = _dbcontext.products.Find(100);
            if (Product is null)    
                return NotFound(new ApiResponse(404));

            return Ok(Product);
        }

        [HttpGet("ServerError")] // Get :  Api/bugge/ServerError
        public ActionResult GetServerErrorResponse()
        {
            var product = _dbcontext.products.Find(100);
            var productToReturn = product.ToString;  //will throw Exception [null ReferenceException ]
            return Ok(productToReturn);
        }

        [HttpGet("badRequest")] // Get :  Api/bugge/badRequest
        public ActionResult GetBadRequst()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badRequest/{id}")]// Get :  Api/bugge/badRequest/Five
        public ActionResult GetBadRequst(int id)//Validation Error
        {
            return Ok();
        }
        [HttpGet("Unauthorized")] // Get :  Api/bugge/badRequest
        public ActionResult GetUnauthriezed()
        {
            return Unauthorized(new ApiResponse(401 ));
        }


    }
}
