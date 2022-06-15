using System;
namespace Library.eCommerce.Models
{
	public class InventoryItemByQuantity : InventoryItem
	{
		public int Quantity { get; set; }
		public InventoryItemByQuantity()
		{
			Quantity = 0;
		}

		public InventoryItemByQuantity(string name, string description, decimal price, int quantity, bool BoGo)
		{
			Name = name;
			Description = description;
			Price = price;
			Quantity = quantity;
			isBoGo = BoGo;
		}

		public InventoryItemByQuantity(InventoryItem invItem, int quantity)
        {
			this.Name = invItem.Name;
			this.Description = invItem.Description;
			this.Price = invItem.Price;
			this.Quantity = quantity;
			this.Id = invItem.Id;
			this.isBoGo = invItem.isBoGo;
        }

		public override string ToString()
		{
			var BoGo = "no";
			if (isBoGo)
				BoGo = "yes";
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Quantity: {Quantity}, BoGo: {BoGo}";
		}

	}
}

