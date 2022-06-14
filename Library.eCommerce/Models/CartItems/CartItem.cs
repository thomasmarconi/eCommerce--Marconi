using System;

namespace Library.eCommerce.Models
{
    public abstract class CartItem
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Weight { get; set; }

        public decimal TotalPrice;

        public int Id { get; set; }
        public CartItem()
        {
            Name = String.Empty;
            Description = String.Empty;
            Price = 0;
        }

        public CartItem(string name, string description, decimal price, int iD)
        {
            Name = name;
            Description = description;
            Price = price;
            Id = iD;
        }


    }
}
