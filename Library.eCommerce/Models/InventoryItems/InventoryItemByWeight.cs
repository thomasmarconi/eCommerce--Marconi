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

		public InventoryItemByWeight(string name, string description, decimal price, decimal weight, bool BoGo)
		{
			Name = name;
			Description = description;
			Price = price;
			Weight = weight;
			isBoGo = BoGo;
		}

		public InventoryItemByWeight(InventoryItem invItem, decimal weight)
		{
			this.Name = invItem.Name;
			this.Description = invItem.Description;
			this.Price = invItem.Price;
			this.Weight = weight;
			this.Id = invItem.Id;
			this.isBoGo = invItem.isBoGo;
		}

		public override string ToString()
		{
			var BoGo = "no";
			if (isBoGo)
				BoGo = "yes";
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Weight: {Weight} lbs, BoGo: {BoGo}";
		}
	}
}

