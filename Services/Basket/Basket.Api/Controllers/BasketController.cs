using AutoMapper;
using Basket.Api.Entites;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        private readonly DiscountGrpcService _discountGrpcService;

        private readonly IPublishEndpoint _publishEndpoint;

        private readonly IMapper _mapper;


        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(_basketRepository));

            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(_discountGrpcService));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(_publishEndpoint));
        }


        [HttpGet("{username}")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await _basketRepository.GetBasketAsync(username);

            if (basket is null) return new NotFoundObjectResult(username);

            return Ok(basket);
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _basketRepository.DeleteBasketAsync(username);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount * item.Quantity;
            }

            var UpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            return Ok(UpdatedBasket);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var cart = await _basketRepository.GetBasketAsync(basketCheckout.Username);

            if (cart == null)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            eventMessage.TotalPrice = cart.TotalPrice;

            await _publishEndpoint.Publish(eventMessage);


            await _basketRepository.DeleteBasketAsync(basketCheckout.Username);

            return Accepted();
        }

    }
}
