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

		public dynamic GetInventoryItemByID(int iD)
        {
			return Inventory.FirstOrDefault(i =>
					(i?.Id.Equals(iD) ?? false)) ?? new InventoryItem();
		}

		public dynamic GetCartItemByID(int iD)
		{
			return Cart.FirstOrDefault(i =>
					(i?.Id.Equals(iD) ?? false)) ?? new CartItem();
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

		public bool AddToCart(int productId, decimal amount)
		{
			var itemToAdd = GetInventoryItemByID(productId);
			if (itemToAdd != null)
			{
				if (itemToAdd is InventoryItemByQuantity)
				{
					if (amount > (itemToAdd?.Quantity ?? 0))
					{
						amount = itemToAdd?.Quantity ?? 0;
						Console.WriteLine($"Error: Not enough {itemToAdd.Name ?? string.Empty}s in inventory. Adding maximum({amount}) {itemToAdd.Name}s to cart instead.");
						if (itemToAdd != null)
						{
							//set inventory item's quantity = 0
							(Inventory.FirstOrDefault(i => (i?.Id.Equals(itemToAdd.Id) ?? false)) as InventoryItemByQuantity).Quantity = 0;
						}
					}
					

					//make the product to add for later just in case
					var addToCart = new CartItemByQuantity(itemToAdd?.Name ?? String.Empty, itemToAdd?.Description ?? String.Empty, itemToAdd?.Price ?? 0, (int)amount, itemToAdd?.Id ?? 0, itemToAdd?.isBoGo ?? false);
					if (Cart.Count == 0) //if cart is empty just add
					{
						Cart.Add(addToCart);
						return true;
					}
					else //see if cart already has the product in it
					{
						for (int i = 0; i < Cart.Count; i++)
						{
							if (Cart[i].Name == itemToAdd?.Name ?? string.Empty)
							{
								var cartItem = (Cart[i] as CartItemByQuantity);
								if (cartItem != null)
									cartItem.Quantity += (int)amount;
								return true;
							}
						}
					}
					//cart doesnt contain product already, add it to cart
					Cart.Add(addToCart);
					return true;
				}
				else if (itemToAdd is InventoryItemByWeight)
				{
					if (amount > (itemToAdd?.Weight ?? 0))
					{
						amount = itemToAdd?.Weight ?? 0;
						Console.WriteLine($"Error: Not enough {itemToAdd.Name ?? string.Empty}s in inventory. Adding maximum({amount}) {itemToAdd.Name}s to cart instead.");
						if (itemToAdd != null)
						{
							//set inventory item's quantity = 0
							(Inventory.FirstOrDefault(i => (i?.Id.Equals(itemToAdd.Id) ?? false)) as InventoryItemByQuantity).Quantity = 0;
						}
					}

					//make the product to add for later just in case
					var addToCart = new CartItemByWeight(itemToAdd?.Name ?? String.Empty, itemToAdd?.Description ?? String.Empty, itemToAdd?.Price ?? 0, amount, itemToAdd?.Id ?? 0, itemToAdd?.isBoGo ?? false );
					if (Cart.Count == 0) //if cart is empty just add
					{
						Cart.Add(addToCart);
						return true;
					}
					else //see if cart already has the product in it
					{
						for (int i = 0; i < Cart.Count; i++)
						{
							if (Cart[i].Name == itemToAdd.Name)
							{
								var cartItem = (Cart[i] as CartItemByWeight);
								if (cartItem != null)
									cartItem.Weight+= amount;
								return true;
							}
						}
					}
					//cart doesnt contain product already, add it to cart
					Cart.Add(addToCart);
					return true;
				}
				else return false;

			}
			else return false; 
		}

		public bool DeleteFromCart(string product, decimal quantity)
		{
			if (Cart.Count == 0)
				return false;
			else
			{
				for (int i = 0; i < Cart.Count; i++)
				{
					if (Cart[i].Name == product)
					{
						if(Cart[i] is CartItemByQuantity)
                        {
							var cartItem = Cart[i] as CartItemByQuantity;
							if (cartItem != null)
							{
								cartItem.Quantity -= (int)quantity;
								if (cartItem.Quantity < 1)
									Cart.RemoveAt(i);
								return true;
							}
						}
						else if (Cart[i] is CartItemByWeight)
                        {
							var cartItem = Cart[i] as CartItemByWeight;
							if (cartItem != null)
							{
								cartItem.Weight -= quantity;
								if (cartItem.Weight < 1)
									Cart.RemoveAt(i);
								return true;
							}
						}
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
			PaymentMethod();
		}

		private void PaymentMethod()
		{
			Console.WriteLine("Choose Payment Method:");
			Console.WriteLine("1. Credit Card");
			Console.WriteLine("2. Subway Gift Card");
			Console.WriteLine("3. TJMaxx Gift Card");
			Console.WriteLine("4. Cash");
			var choice = Console.ReadLine() ?? string.Empty;
			switch ((choice))
			{
				case "1":
					{
						Console.WriteLine("Enter Credit Cart Number: ");
						var creditCardNumber = Console.ReadLine() ?? string.Empty;
						Console.WriteLine("PIN: ");
						var securityCode = Console.ReadLine() ?? string.Empty;
						Console.WriteLine("Enter Expiration (MM/YYYY): ");
						var expiration = Console.ReadLine() ?? string.Empty;
						break;
					}

				case "2":
					{
						Console.WriteLine("Enter Gift Card Number: ");
						var creditCardNumber = Console.ReadLine() ?? string.Empty;
						Console.WriteLine("Enter PIN: ");
						var PIN = Console.ReadLine() ?? string.Empty;
						break;
					}

				case "3":
					{
						Console.WriteLine("Enter Gift Card Number: ");
						var creditCardNumber = Console.ReadLine() ?? string.Empty;
						Console.WriteLine("Enter PIN: ");
						var PIN = Console.ReadLine() ?? string.Empty;
						break;
					}

				case "4":
					{
						Console.WriteLine("Enter Cash Amount: ");
						var creditCardNumber = Console.ReadLine() ?? string.Empty;
						Console.WriteLine("Open Disc Drive and Jam Money Inside...it should be fineeeee");
						break;
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

		public void SetBoGoStatus(InventoryItem item)
        {
			Console.WriteLine("Enter BoGo status: (yes or no)");
			var BoGo = Console.ReadLine() ?? String.Empty;
			item.isBoGo = BoGo == "yes";
        }

		//implement ListNav
		private string query;
		private SortType sort;
		public void Search(string query)
        {
			this.query = query;
        }

		public IEnumerable<InventoryItem> ProcessedList
        {
            get
            {
				if (string.IsNullOrEmpty(this.query))
					return Inventory;
				else 
					return Inventory
					.Where(i => string.IsNullOrEmpty(this.query) ||( (i?.Name?.ToUpper()?.Contains(this.query.ToUpper()) ?? false)
					|| (i?.Description?.ToUpper()?.Contains(this.query.ToUpper()) ?? false))) //search -- filter
					.OrderBy(i => this.sort);													//sort -- order
            }
        }

		public enum SortType
        {
			Name, TotalPrice, UnitPrice
        }
	}
}