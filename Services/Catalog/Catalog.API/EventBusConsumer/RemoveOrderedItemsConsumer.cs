using Catalog.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;

namespace Catalog.API.EventBusConsumer
{
    public class RemoveOrderedItemsConsumer : IConsumer<RemoveOrderedItemsEvent>
    {
        private readonly IProductRepository _repository;

        public RemoveOrderedItemsConsumer(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Consume(ConsumeContext<RemoveOrderedItemsEvent> context)
        {
            var orderedItems = context.Message.Items;

            foreach (var item in orderedItems)
            {
                var currentBook = await _repository.GetProduct(item.BookId);

                if (currentBook == null)
                {
                    // Handle the case where the book is not found (optional).
                    // You may log this or take other actions. To inform the user that this book is not available
                }
                else if (currentBook.AvailableQuantity >= item.Quantity)
                {
                    currentBook.AvailableQuantity -= item.Quantity;
                    _repository.UpdateProduct(currentBook);
                }
                else
                {
                    // Handle the case where the available quantity is not sufficient (optional).
                    // You may log this or take other actions. To inform the user that this book is out of stock
                }
            }
        }

    }
}
