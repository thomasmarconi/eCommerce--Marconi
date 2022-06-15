using System;
namespace Library.eCommerce.Models
{
	public class CartItemByWeight : CartItem
	{
		public decimal Weight { get; set; }
		public override decimal TotalPrice
		{
			get
			{
				if(isBoGo && Weight > 1)
					return ((Weight/2) * Price);
				else 
					return (Weight * Price);
			}
		}
		public CartItemByWeight()
		{
		}

		public CartItemByWeight(string name, string description, decimal price, decimal weight, int iD, bool BoGo)
		{
			Name = name;
			Description = description;
			Price = price;
			Weight = weight;
			Id = iD;
			isBoGo = BoGo;
		}

		public override string ToString()
		{
			var BoGo = "no";
			if (isBoGo)
				BoGo = "yes";
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Weight: {Weight} lbs, BoGo: {BoGo} Total Price: {TotalPrice}";
		}
	}
}

