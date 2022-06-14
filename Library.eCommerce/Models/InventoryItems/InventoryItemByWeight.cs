using System;
namespace Library.eCommerce.Models
{
	public class InventoryItemByWeight : InventoryItem
	{
		public decimal Weight { get; set; }
		public InventoryItemByWeight()
		{
			Weight = 0;
		}

		public InventoryItemByWeight(string name, string description, decimal price, decimal weight)
		{
			Name = name;
			Description = description;
			Price = price;
			Weight = weight;
		}

		public InventoryItemByWeight(InventoryItem invItem, decimal weight)
		{
			this.Name = invItem.Name;
			this.Description = invItem.Description;
			this.Price = invItem.Price;
			this.Weight = weight;
			this.Id = invItem.Id;
		}

		public override string ToString()
		{
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Weight: {Weight} lbs";
		}
	}
}

