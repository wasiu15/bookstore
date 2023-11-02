using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            // get basket with username
            // iterate basket items and consume products with basket item productId member
            // map product related members into basketitem dto with extended columns
            // consume ordering microservices in order to retrieve order list
            // return root ShoppingModel dto class which including all responses

            var basket = await _basketService.GetBasket(userName);
            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.BookId);

                // set additional product fields onto basket item
                item.BookName = product.Title;
                item.Genre = product.Genre;
                item.AboutAuthor = product.AboutAuthor;
                item.CoverImageUrl = product.CoverImageUrl;
                item.PublicationDate = product.PublicationDate;
            }

            var orders = await _orderService.GetOrdersByUserName(userName);

            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }

        [HttpGet("/books", Name = "GetBooks")]
        [ProducesResponseType(typeof(IEnumerable<CatalogModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CatalogModel>>> GetProducts()
        {
            var products = await _catalogService.GetCatalogs();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CatalogModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CatalogModel>> GetProductById(string id)
        {
            var product = await _catalogService.GetCatalog(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        ///////
        /// GET ORDER STATUS... JUST ADD PROCESSING TO IT HERE... FULL DTO AND INCLUDE STATUS PROCESSING TO IT HERE
    }
}
