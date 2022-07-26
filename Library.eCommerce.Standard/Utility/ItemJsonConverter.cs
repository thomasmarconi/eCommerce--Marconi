
using Library.eCommerce.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.eCommerce.Standard.Utility
{
    public class ItemJsonConverter : JsonCreationConverter<Item>
    {
        protected override Item Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["totalPrice"] != null || jObject["TotalPrice"] != null) // is cartItem
            {
                if (jObject["quantity"] != null || jObject["Quantity"] != null)
                    return new CartItemByQuantity();
                else if (jObject["weight"] != null || jObject["weight"] != null)
                    return new CartItemByWeight();
                else
                    return new CartItem();
            }
            else if (jObject["inventory"] != null || jObject["Inventory"] != null) //is inventoryItem
            {
                if (jObject["quantity"] != null || jObject["Quantity"] != null)
                    return new InventoryItemByQuantity();
                else if (jObject["weight"] != null || jObject["weight"] != null)
                    return new InventoryItemByWeight();
                else
                    return new InventoryItem();
            }
            else
            {
                return new Item();
            }
        }
    }
}
