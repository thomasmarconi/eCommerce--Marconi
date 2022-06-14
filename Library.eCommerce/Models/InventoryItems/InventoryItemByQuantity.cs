using System;
namespace Library.eCommerce.Models
{
	public class InventoryItemByQuantity : InventoryItem
	{
		
		public InventoryItemByQuantity()
		{
			Quantity = 0;
		}

		public InventoryItemByQuantity(string name, string description, decimal price, int quantity, int iD)
		{
			Name = name;
			Description = description;
			Price = price;
			Quantity = quantity;
			Id = iD;
		}

		public InventoryItemByQuantity(InventoryItem invItem, int quantity)
        {
			this.Name = invItem.Name;
			this.Description = invItem.Description;
			this.Price = invItem.Price;
			this.Quantity = quantity;
			this.Id = invItem.Id;
        }

		public override string ToString()
		{
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Quantity: {Quantity}";
		}

	}
}

