using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] //POST: /api/Orders
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

           var order =  await orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, address, orderDto.DeliveryMethodId);

            if (order is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await orderService.GetOrdersForUserAsync(BuyerEmail);

            var mappedOrders = mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(Orders);

            return Ok(mappedOrders);
        }
        [HttpGet("{id}")]//GET: /api/Orders/2
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var order = await orderService.GetOrderByIdForUserAsync(id, BuyerEmail);

            if (order is null) return NotFound(new ApiErrorResponse(404));

            var mappedOrder = mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }

        [HttpGet("deliverymethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
