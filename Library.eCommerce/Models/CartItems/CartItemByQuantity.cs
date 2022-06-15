using System;
namespace Library.eCommerce.Models
{
	public class CartItemByQuantity : CartItem
	{
		public int Quantity { get; set; }
		public override decimal TotalPrice
		{
			get
			{
				if (isBoGo)
					return ((Quantity / 2) * Price);
				else
					return (Quantity * Price);
			}
		}

		public CartItemByQuantity()
		{
		}

		public CartItemByQuantity(string name, string description, decimal price, int quantity, int iD, bool BoGo)
		{
			Name = name;
			Description = description;
			Price = price;
			Quantity = quantity;
			Id = iD;
			isBoGo = BoGo;
		}

		public override string ToString()
		{
			var BoGo = "no";
			if (isBoGo)
				BoGo = "yes";
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Quantity: {Quantity}, BoGo: {BoGo} Total Price: {TotalPrice}";
		}
	}
}

