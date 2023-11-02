using EventBus.Messages.Events;

namespace Basket.API.Entities
{
    public class RemoveOrderedItemsEvent_
    {
        public List<ShoppingCartItem> Items { get; set; }

    }
}
