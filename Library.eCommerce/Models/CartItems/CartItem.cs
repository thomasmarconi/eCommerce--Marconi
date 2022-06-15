using System;

namespace Library.eCommerce.Models
{
    public class CartItem
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }   
        public bool isBoGo { get; set; }
     

        public virtual decimal TotalPrice { get; set; }

        public int Id { get; set; }
        public CartItem()
        {
            Name = String.Empty;
            Description = String.Empty;
            Price = 0;
            isBoGo = false;
        }

        public CartItem(string name, string description, decimal price, int iD)
        {
            Name = name;
            Description = description;
            Price = price;
            Id = iD;
            isBoGo = false;
        }


    }
}
