using System;
namespace Library.eCommerce.Models
{
	public class CartItemByQuantity : CartItem
	{
		
		public new decimal TotalPrice
		{
			get
			{
				return (Quantity * Price);
			}
		}

		public CartItemByQuantity()
		{
		}

		public CartItemByQuantity(string name, string description, decimal price, int quantity, int iD)
		{
			Name = name;
			Description = description;
			Price = price;
			Quantity = quantity;
			Id = iD;
		}

		public override string ToString()
		{
			return $"#{Id}. {Name} :: {Description} -- Price: {Price}, Quantity: {Quantity}, Total Price: {TotalPrice}";
		}
	}
}

