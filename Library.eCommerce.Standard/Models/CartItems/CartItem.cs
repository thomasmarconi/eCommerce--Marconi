using Library.eCommerce.Standard.Utility;
using Newtonsoft.Json;
using System;

namespace Library.eCommerce.Models
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class CartItem : Item
    {
        public virtual double TotalPrice { get; set; }
        
        public CartItem()
        {
            Name = String.Empty;
            Description = String.Empty;
            Price = 0;
            isBoGo = false;
        }
    }
}
