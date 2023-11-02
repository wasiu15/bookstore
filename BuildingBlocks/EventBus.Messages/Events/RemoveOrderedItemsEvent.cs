using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class RemoveOrderedItemsEvent : IntegrationBaseEvent
    {
        public List<ShoppingCartItem> Items { get; set; }

    }
    public class ShoppingCartItem
    {
        public string BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
