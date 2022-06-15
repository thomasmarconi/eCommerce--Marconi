using System;
namespace Library.eCommerce.Models
{
	public class InventoryItem
	{
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool isBoGo { get; set; }
        
        


        public int Id { get; set; }
        public InventoryItem()
        {
            Name = String.Empty;
            Description = String.Empty;
            Price = 0;
            isBoGo = false;
        }
    }
}

