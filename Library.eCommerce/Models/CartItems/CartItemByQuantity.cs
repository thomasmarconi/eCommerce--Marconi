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
				if (isBoGo && Quantity > 1)
					return ((Quantity / 2) * Price);
				else
					return (Quantity * Price);
			}
		}

		public CartItemByQuantity()
		{
		}

		public CartItemByQuantity(string name, string description, decimal price, int quantity, bool BoGo, int iD = 0)
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
			var BoGo = "No";
			if (isBoGo)
				BoGo = "Yes";
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Quantity: {Quantity}, BoGo: {BoGo} Total Price: {TotalPrice}";
		}
	}
}

