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
				return (Weight * Price);
			}
		}
		public CartItemByWeight()
		{
		}

		public CartItemByWeight(string name, string description, decimal price, decimal weight, int iD)
		{
			Name = name;
			Description = description;
			Price = price;
			Weight = weight;
			Id = iD;
		}

		public override string ToString()
		{
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Weight: {Weight}, Total Price: {TotalPrice} lbs";
		}
	}
}

