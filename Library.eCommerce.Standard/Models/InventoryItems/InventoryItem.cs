using System;
namespace Library.eCommerce.Models
{
	public class InventoryItem : Item
	{
        public InventoryItem()
        {
            Name = String.Empty;
            Description = String.Empty;
            Price = 0;
            isBoGo = false;
        }
    }
}

