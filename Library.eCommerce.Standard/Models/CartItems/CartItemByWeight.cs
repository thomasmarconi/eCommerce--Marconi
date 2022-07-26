using Library.eCommerce.Standard.Utility;
using Newtonsoft.Json;
using System;
namespace Library.eCommerce.Models
{
	[JsonConverter(typeof(ItemJsonConverter))]
	public class CartItemByWeight : CartItem
	{
		public double Weight { get; set; }
		public override double TotalPrice
		{
			get
			{
				if (isBoGo && Weight > 1 && Weight % 2 == 0)
					return (((int)Weight / 2) * Price);
				else if (isBoGo && Weight > 1 && Weight % 2 != 0)
					return (((int)Weight / 2 + 1) * Price);
				else
					return (Weight * Price);
			}
		}
		public CartItemByWeight()
		{
		}

		public CartItemByWeight(string name, string description, double price, double weight, bool BoGo, int iD = 0)
		{
			Name = name;
			Description = description;
			Price = price;
			Weight = weight;
			Id = iD;
			isBoGo = BoGo;
		}

		public CartItemByWeight(InventoryItemByWeight item, double weight)
        {
			Name = item.Name;
			Description = item.Description;
			Price = item.Price;
			isBoGo = item.isBoGo;
			Weight = weight;
			Id= item.Id;
		}

		public override string ToString()
		{
			var BoGo = "No";
			if (isBoGo)
				BoGo = "Yes";
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Weight: {Weight} lbs, BoGo: {BoGo} Total Price: {TotalPrice}";
		}
	}
}

