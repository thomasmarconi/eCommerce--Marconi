using Library.eCommerce.Standard.Utility;
using Newtonsoft.Json;
using System;
namespace Library.eCommerce.Models
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class InventoryItem : Item
	{
        public String Inventory;
        public InventoryItem()
        {
            Name = String.Empty;
            Description = String.Empty;
            Price = 0;
            isBoGo = false;
            Inventory = "yes";
        }
    }
}

