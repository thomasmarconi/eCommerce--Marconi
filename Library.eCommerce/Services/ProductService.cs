using System;
using Library.eCommerce.Models;
using Newtonsoft.Json;

namespace Library.ECommerce.Services
{
	public class ProductService
	{
		//data
		private List<InventoryItem> inventoryList;
		public List<InventoryItem> Inventory
		{
			get { return inventoryList; }
		}

		private List<CartItem> cartList;
		public List<CartItem> Cart
		{
			get { return cartList; }
		}

		private static ProductService? current;
		public static ProductService Current
		{
			get
			{
				if (current == null)
					current = new ProductService();
				return current;
			}
		}

		//methods
		private ProductService()
		{
			inventoryList = new List<InventoryItem>();
			cartList = new List<CartItem>();
		}

		public IEnumerable<InventoryItem> GetFilteredList(string? query)
		{
			if (string.IsNullOrEmpty(query))
			{
				return Inventory;
			}
			return Inventory.Where(i =>
					(i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
					|| (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
		}

		public IEnumerable<CartItem> GetFilteredListCart(string? query)
		{
			if (string.IsNullOrEmpty(query))
			{
				return Cart;
			}
			return Cart.Where(i =>
					(i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
					|| (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
		}

		public void AddOrUpdate(InventoryItem item)
		{
			//Id management for adding a new record.
			if (item.Id == 0)
			{
				if (inventoryList.Any())
				{
					item.Id = inventoryList.Select(i => i.Id).Max() + 1;
				}
				else
				{
					item.Id = 1;
				}
			}

			if (!inventoryList.Any(i => i.Id == item.Id))
			{
				inventoryList.Add(item);
			}
		}

		public void Delete(int id)
		{
			var itemToDelete = inventoryList.FirstOrDefault(i => i.Id == id);
			if (itemToDelete != null)
			{
				inventoryList.Remove(itemToDelete);
			}
		}

		public void DeleteFromCart(int id)
		{
			var itemToDelete = cartList.FirstOrDefault(i => i.Id == id);
			if (itemToDelete != null)
			{
				cartList.Remove(itemToDelete);
			}
		}

		public bool AddToCart(int productId)
		{



			bool inInventory = false;
			var remFromInv = new InventoryItemByQuantity();
			for (int i = 0; i < Inventory.Count; i++)
			{
				if (Inventory[i].Id == productId)
				{
					if (quantity > Inventory[i].Quantity)
					{
						quantity = Inventory[i].Quantity;
						Console.WriteLine($"Error: Not enough {Inventory[i].Name}s in inventory. Adding maximum({quantity}) {Inventory[i].Name}s to cart instead.");
					}
					Inventory[i].Quantity--;
					remFromInv = (InventoryItemByQuantity)Inventory[i];
					inInventory = true;
				}
			}
			//make the product to add for later just in case
			var addToCart = new CartItemByQuantity(remFromInv.Name ?? String.Empty, remFromInv.Description ?? String.Empty, remFromInv.Price, quantity, remFromInv.Id);
			if (inInventory == false)
				return false;
			else if (Cart.Count == 0) //if cart is empty just add
			{
				Cart.Add(addToCart);
				return true;
			}
			else //see if cart already has the product in it
			{
				for (int i = 0; i < Cart.Count; i++)
				{
					if (Cart[i].Name == remFromInv.Name)
					{
						Cart[i].Quantity++;
						return true;
					}
				}
			}
			//cart doesnt contain product already, add it to cart
			Cart.Add(addToCart);
			return true;
		}

		public bool AddToCart(int productId, decimal weight)
		{
			bool inInventory = false;
			var remFromInv = new InventoryItemByWeight();
			for (int i = 0; i < Inventory.Count; i++)
			{
				if (Inventory[i].Id == productId)
				{
					if (weight > Inventory[i].Weight)
					{
						weight = Inventory[i].Weight;
						Console.WriteLine($"Error: Not enough {Inventory[i].Name}s in inventory. Adding maximum({weight}) {Inventory[i].Name}s to cart instead.");
					}
					Inventory[i].Weight--;
					remFromInv = (InventoryItemByWeight)Inventory[i];
					inInventory = true;
				}
			}
			//make the product to add for later just in case
			var addToCart = new CartItemByWeight(remFromInv.Name ?? String.Empty, remFromInv.Description ?? String.Empty, remFromInv.Price, weight, remFromInv.Id);
			if (inInventory == false)
				return false;
			else if (Cart.Count == 0) //if cart is empty just add
			{
				Cart.Add(addToCart);
				return true;
			}
			else //see if cart already has the product in it
			{
				for (int i = 0; i < Cart.Count; i++)
				{
					if (Cart[i].Name == remFromInv.Name)
					{
						Cart[i].Quantity++;
						return true;
					}
				}
			}
			//cart doesnt contain product already, add it to cart
			Cart.Add(addToCart);
			return true;
		}

		public bool DeleteFromCart(string product, int quantity)
		{
			if (Cart.Count == 0)
				return false;
			else
			{
				for (int i = 0; i < Cart.Count; i++)
				{
					if (Cart[i].Name == product)
					{
						Cart[i].Quantity -= quantity;
						if (Cart[i].Quantity < 1)
							Cart.RemoveAt(i);
						return true;
					}
				}
				return false;
			}
		}

		public void Checkout()
		{
			decimal subtotal = 0;
			foreach (var product in Cart)
			{
				Console.WriteLine($"{product}");
				subtotal += product.TotalPrice;
			}
			var taxAmount = subtotal * (decimal).07;
			Console.WriteLine("----------------------------------");
			Console.WriteLine($"Subtotal is: {subtotal}");
			Console.WriteLine($"Tax is: {taxAmount}");
			Console.WriteLine($"Total is: {subtotal + taxAmount}\n");
		}

		public void SearchByName(string InvOrCart, string query)
		{
			if (InvOrCart == "Inv")
			{
				for (int i = 0; i < Inventory.Count; i++)
				{
					if (Inventory[i].Name == query)
					{
						Console.WriteLine(Inventory[i]);
					}
				}
			}
			else //searching the cart
			{
				for (int i = 0; i < Cart.Count; i++)
				{
					if (Cart[i].Name == query)
					{
						Console.WriteLine(Cart[i]);
					}
				}
			}
		}

		public void SearchByDesc(string InvOrCart, string query)
		{
			if (InvOrCart == "Inv")
			{
				for (int i = 0; i < Inventory.Count; i++)
				{
					if (Inventory[i].Description == query)
					{
						Console.WriteLine(Inventory[i]);
					}
				}
			}
			else //searching the cart
			{
				for (int i = 0; i < Cart.Count; i++)
				{
					if (Cart[i].Description == query)
					{
						Console.WriteLine(Cart[i]);
					}
				}
			}

		}

		public void Load()
		{
			var productInvJson = File.ReadAllText("SaveDataInv.json");
			var productCartJson = File.ReadAllText("SaveDataCart.json");
			inventoryList = JsonConvert.DeserializeObject<List<InventoryItem>>(productInvJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) ?? new List<InventoryItem>();
			cartList = JsonConvert.DeserializeObject<List<CartItem>>(productCartJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) ?? new List<CartItem>();
		}

		public void Save()
		{
			var productInvJson = JsonConvert.SerializeObject(Inventory, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
			var productCartJson = JsonConvert.SerializeObject(Cart, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
			File.WriteAllText("SaveDataInv.json", productInvJson);
			File.WriteAllText("SaveDataCart.json", productCartJson);
		}
	}
}