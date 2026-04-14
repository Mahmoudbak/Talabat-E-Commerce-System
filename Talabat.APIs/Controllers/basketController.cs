using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Error;
using Talabat.Core.Entity.Basket;
using Talabat.Core.Repository.contrent;

namespace Talabat.APIs.Controllers
{

    public class basketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public basketController(IBasketRepository basketRepository,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        //[HttpGet] // Api/Basket?id
        //public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        //{
        //    var basket = await _basketRepository.GetBasketAsync(id);
        //    return Ok(basket is null ? new CustomerBasket(id) : basket);
        //}


        [HttpGet] // Api/Basket?id
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket  ?? new CustomerBasket(id));
        }


        [HttpPost] // POST  Api/Basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var CreateOrUpdateBasket = await _basketRepository.UpdateBasketAsync(basket);
            if (CreateOrUpdateBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreateOrUpdateBasket);
        }


        //[HttpPost] // POST  Api/Basket
        //public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        //{

        //    var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

        //    var CreateOrUpdateBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);

        //    if (CreateOrUpdateBasket is null) return BadRequest(new ApiResponse(400));

        //    return Ok(CreateOrUpdateBasket);
        //}

        //[HttpDelete] //Delete Api/basket?id    
        //public async Task<ActionResult<bool>> DeleteBasket(string id)
        //{
        //    return await _basketRepository.DeleteBasketAsync(id); 
        //}
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }

    }    
       
}
    
    
    
    
    
    
    
    
    
    
