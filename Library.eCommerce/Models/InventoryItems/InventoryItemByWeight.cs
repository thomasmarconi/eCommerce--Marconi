using System;
namespace Library.eCommerce.Models
{
	public class InventoryItemByWeight : InventoryItem
	{
		
		public InventoryItemByWeight()
		{
			Weight = 0;
		}

		public InventoryItemByWeight(string name, string description, decimal price, decimal weight, int iD)
		{
			Name = name;
			Description = description;
			Price = price;
			Weight = weight;
			Id = iD;
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
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Weight: {Weight}";
		}
	}
}

