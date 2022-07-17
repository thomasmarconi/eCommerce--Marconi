using System;
namespace Library.eCommerce.Models
{
	public class CartItemByQuantity : CartItem
	{
		public int Quantity { get; set; }
		public override double TotalPrice
		{
			get
			{
				if (isBoGo && Quantity > 1 && Quantity % 2 == 0)
					return ((Quantity / 2) * Price);
				else if (isBoGo && Quantity > 1 && Quantity % 2 == 1)
					return ((Quantity / 2 + 1) * Price);
				else
					return (Quantity * Price);
			}
		}

		public CartItemByQuantity()
		{
		}

		public CartItemByQuantity(string name, string description, double price, int quantity, bool BoGo, int iD = 0)
		{
			Name = name;
			Description = description;
			Price = price;
			Quantity = quantity;
			Id = iD;
			isBoGo = BoGo;
		}

		public CartItemByQuantity(InventoryItemByQuantity item, int quantity)
        {
			Name = item.Name;
			Description = item.Description;
			Price = item.Price;
			isBoGo = item.isBoGo;
			Quantity = quantity;
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

