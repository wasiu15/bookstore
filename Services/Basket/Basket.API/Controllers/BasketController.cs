using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // 1. Get the existing basket with the total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);

            // 2. If the basket does not exist, return a Bad Request response
            if (basket == null)
            {
                return BadRequest();
            }

            // 3. Create a BasketCheckoutEvent and set the TotalPrice on the eventMessage
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            // 4. Send the checkout event to RabbitMQ
            await _publishEndpoint.Publish(eventMessage);

            // 5. Create a shoppingCart and populate it with items from the basket
            var shoppingCart = new ShoppingCart();
            shoppingCart.Items.AddRange(basket.Items);

            // 6. Create a RemoveOrderedItemsEvent and map the shoppingCart to it
            var eventBasket = _mapper.Map<RemoveOrderedItemsEvent>(shoppingCart);

            // 7. Send the ordered items list event to RabbitMQ so that they can be removed from the stock
            await _publishEndpoint.Publish(eventBasket);

            // 8. Remove the basket from the repository
            await _repository.DeleteBasket(basket.UserName);

            // 9. Return an Accepted response to indicate the successful completion of the process
            return Accepted();

        }
    }
}
