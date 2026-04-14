using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Error;
using Talabat.Core.Entity.Order_Aggreate;
using Talabat.Core.Services.Content;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(
            IOrderService orderService,
            IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        //[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        //[HttpPost]// POST  /Api/ orders
        //public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDTO orderDto)
        //{
        //    var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        [HttpPost]// POST  /Api/ orders
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDto)
        {
            string buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orderAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, orderAddress);

            if (order == null) return BadRequest(new ApiResponse(400, "An error occured during the creation of the order"));

            return Ok(order);
        }

        //    var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

        //    var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliverMethodId, address);

        //    if (order is null) return BadRequest(new ApiResponse(400));

        //    return Ok(_mapper.Map< OrderToReturnDto>(order));


        //}

        //    [HttpGet] // GET : /Api/orders?email=mahmoud14@gmail.com
        //public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser(string email)
        //{
        //    var orders = await _orderService.GetOrderForUserAsync(email);

        //    return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList< OrderToReturnDto>>(orders));
        //}

        [HttpGet] // GET : /Api/orders/id
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail=User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrderForUserAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }




        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] // GET : /api/orders/1?email=mahmoud14@gmail.com
        public async Task<ActionResult<OrderToReturnDto>> GetOrdersForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.CreateOrderByIdForUserAsync(id, buyerEmail);
        
            if (order is null) return NotFound(new ApiResponse(404));
            
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        
        
        }

        [Authorize]
        [HttpGet("deliveryMethods")] // Get : api/orders/deliverymethods

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        { 
            var deliveryMethod= await _orderService.GetDeliveryMethodAsync();

            return Ok(deliveryMethod);
        }


    }
}
