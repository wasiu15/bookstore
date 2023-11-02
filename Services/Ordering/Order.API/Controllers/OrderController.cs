using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Net;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get a list of orders by the user's username.
        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByUserName(string userName)
        {
            // Create a query to retrieve the list of orders for the specified user.
            var query = new GetOrdersListQuery(userName);

            // Send the query to the mediator and return the orders as an HTTP response with status OK (200).
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        // Checkout an order with the provided command.
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            // Send the command to the mediator to initiate the order checkout process.
            var result = await _mediator.Send(command);

            // Return the result as an HTTP response with status OK (200).
            return Ok(result);
        }

        // Update an order using the provided command.
        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            // Send the command to the mediator to update the order.
            await _mediator.Send(command);

            // Return a No Content (204) response to indicate a successful update.
            return NoContent();
        }

        // Delete an order by its unique identifier.
        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            // Create a command to delete the order with the specified identifier.
            var command = new DeleteOrderCommand() { Id = id };

            // Send the command to the mediator for order deletion and return a No Content (204) response.
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
