using Library.eCommerce.Standard.Utility;
using Newtonsoft.Json;
using System;
namespace Library.eCommerce.Models
{
	[JsonConverter(typeof(ItemJsonConverter))]
	public class InventoryItemByWeight : InventoryItem
	{
		
		public double Weight { get; set; }
		public InventoryItemByWeight()
		{
			Weight = 0;
		}

		public InventoryItemByWeight(string name, string description, double price, double weight, bool BoGo, int iD = 0)
		{
			Name = name;
			Description = description;
			Price = price;
			Weight = weight;
			isBoGo = BoGo;
			Id = iD;
		}

		public InventoryItemByWeight(InventoryItem invItem, double weight)
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
			var BoGo = "No";
			if (isBoGo)
				BoGo = "Yes";
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Weight: {Weight} lbs, BoGo: {BoGo}";
		}
	}
}

