using System;

namespace Library.eCommerce.Models
{
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
