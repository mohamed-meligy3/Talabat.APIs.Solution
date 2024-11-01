using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet] //GET : api/baskets
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            return basket is null ? new CustomerBasket(id) : basket;
        }
        [HttpPost]//POST : api/Baskets
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
           
            var createdOrUpdatedBasket = await basketRepository.UpdateBasketAsync(mappedBasket);
            if (createdOrUpdatedBasket is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(basket);
        }
        [HttpDelete] //DELETE: api/Baskets
        public async Task<ActionResult<bool>> DeleteBasket(string Id)
        {
            return await basketRepository.DeleteBasketAsync(Id);
        }

    }
}
