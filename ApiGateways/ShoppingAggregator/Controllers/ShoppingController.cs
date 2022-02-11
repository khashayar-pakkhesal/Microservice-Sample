using Microsoft.AspNetCore.Mvc;
using ShoppingAggregator.Contracts;
using ShoppingAggregator.Models;
using System.Net;

namespace ShoppingAggregator.Controllers
{
    [ApiController]
    [Route("Api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogSerivce;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogSerivce, IBasketService basketService, IOrderService orderService)
        {
            _catalogSerivce = catalogSerivce ?? throw new ArgumentNullException(nameof(catalogSerivce));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{username}", Name = "getShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
        {
            var basket = await _basketService.GetBasket(username);

            foreach (var item in basket.Items)
            {
                var product = await _catalogSerivce.GetCatalog(item.ProductId);

                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            var orders = await _orderService.GetOrdersByUsername(username);

            var shoppingModel = new ShoppingModel
            {
                Username = username,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);

        }
    }
}
