using System;
using Library.eCommerce.Models;
using Newtonsoft.Json;
using ListNavigator;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Library.eCommerce.Services
{
	public class InventoryService
	{
		//data

		private string persistPath
			= $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";

		private List<InventoryItem> inventoryList;
		public List<InventoryItem> Inventory
		{
			get { return inventoryList; }
		}

		

		private static InventoryService current;
		public static InventoryService Current
		{
			get
			{
				if (current == null)
					current = new InventoryService();
				return current;
			}
		}

		//methods
		private InventoryService()
		{
			inventoryList = new List<InventoryItem>();
			query = String.Empty;
		}

		public IEnumerable<InventoryItem> GetFilteredList(string query) //depricated
		{
			if (string.IsNullOrEmpty(query))
			{
				return Inventory;
			}
			return Inventory.Where(i =>
					(i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
					|| (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
		}

		public int SelectInventoryItem(string action)
		{
			ListItems(Inventory);
			while (true)
			{
				Console.WriteLine($"Which inventory item would you like to {action}?");
				if (int.TryParse(Console.ReadLine(), out var id))
				{
					return id;
				}
			}

		}

		public InventoryItem GetInventoryItemByID(int iD)
        {
			return Inventory.FirstOrDefault(i =>
					(i?.Id.Equals(iD) ?? false)) ?? new InventoryItem();
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
            if (item is InventoryItemByQuantity)
            {
				var oldVersion = inventoryList.FirstOrDefault(i =>i.Id.Equals(item.Id));
				if(oldVersion != null)
                {
					var index = inventoryList.IndexOf(oldVersion);
					inventoryList.RemoveAt(index);
					inventoryList.Insert(index, item);
                }
                else
                {
					inventoryList.Add(item);
                }
            }
            else if(item is InventoryItemByWeight)
            {
				var oldVersion = inventoryList.FirstOrDefault(i => i.Id.Equals(item.Id));
				if (oldVersion != null)
				{
					var index = inventoryList.IndexOf(oldVersion);
					inventoryList.RemoveAt(index);
					inventoryList.Insert(index, item);
				}
				else
				{
					inventoryList.Add(item);
				}
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

		public bool AddToCart(int productId, CartService cart)
		{
			var itemToAdd = GetInventoryItemByID(productId);
			if (itemToAdd == null)
			{
				return false;
			}
            else
            {
				if (itemToAdd is InventoryItemByQuantity)
				{
					var newItemToAdd = itemToAdd as InventoryItemByQuantity;
					if (newItemToAdd != null)
					{
						Console.WriteLine($"How many {newItemToAdd.Name}s would you like to add to the cart?");
						var amountToAdd = int.Parse(Console.ReadLine() ?? string.Empty);
						if(amountToAdd <= 0)
							return false;
						if (amountToAdd > newItemToAdd.Quantity)
						{
							Console.WriteLine($"Not enough {newItemToAdd.Name}s in inventory. Adding max({newItemToAdd.Quantity}) items to cart.");
							amountToAdd = newItemToAdd.Quantity;
						}
						decreaseItemQuantity(GetInventoryItemByID(productId) as InventoryItemByQuantity ?? new InventoryItemByQuantity(), amountToAdd);
						if (cart.Cart.Count == 0) //cart is empty
						{
							cart.AddOrUpdate(new CartItemByQuantity(newItemToAdd.Name ?? String.Empty, newItemToAdd.Description ?? String.Empty, newItemToAdd.Price, amountToAdd, newItemToAdd.isBoGo));
							return true;
						}
						else if (cart.HasItemInCart(newItemToAdd.Name ?? string.Empty) == 0) //cart doesn't contain the item
						{
							cart.AddOrUpdate(new CartItemByQuantity(newItemToAdd.Name ?? String.Empty, newItemToAdd.Description ?? String.Empty, newItemToAdd.Price, amountToAdd, newItemToAdd.isBoGo));
							return true;
						}
						else if (cart.HasItemInCart(newItemToAdd.Name ?? string.Empty) != 0) //cart contains the item
						{
							cart.AddOrUpdate(new CartItemByQuantity(newItemToAdd.Name ?? String.Empty, newItemToAdd.Description ?? String.Empty, newItemToAdd.Price, amountToAdd + cart.GetCartItemQuantity(cart.GetCartItemByID(cart.HasItemInCart(newItemToAdd.Name ?? string.Empty)) as CartItemByQuantity ?? new CartItemByQuantity()), newItemToAdd.isBoGo, cart.HasItemInCart(newItemToAdd.Name ?? string.Empty)));
							return true;
						}
						else
							return false;
					}
					else
						return false;
				}
				else if (itemToAdd is InventoryItemByWeight)
				{
					var newItemToAdd = itemToAdd as InventoryItemByWeight;
					if (newItemToAdd != null)
					{
						Console.WriteLine($"How many lbs of {newItemToAdd.Name} would you like to add to the cart?");
						var amountToAdd = double.Parse(Console.ReadLine() ?? string.Empty);
						if (amountToAdd <= 0)
							return false;
						if (amountToAdd > newItemToAdd.Weight)
						{
							Console.WriteLine($"Not enough {newItemToAdd.Name}s in inventory. Adding max({newItemToAdd.Weight}) lbs to cart.");
							amountToAdd = newItemToAdd.Weight;
						}
						decreaseItemWeight(GetInventoryItemByID(productId) as InventoryItemByWeight ?? new InventoryItemByWeight(), amountToAdd);
						if (cart.Cart.Count == 0) //cart is empty
						{
							cart.AddOrUpdate(new CartItemByWeight(newItemToAdd.Name ?? String.Empty, newItemToAdd.Description ?? String.Empty, newItemToAdd.Price, amountToAdd, newItemToAdd.isBoGo));
							return true;
						}
						else if (cart.HasItemInCart(newItemToAdd.Name ?? string.Empty) == 0) //cart doesn't contain the item
						{
							cart.AddOrUpdate(new CartItemByWeight(newItemToAdd.Name ?? String.Empty, newItemToAdd.Description ?? String.Empty, newItemToAdd.Price, amountToAdd, newItemToAdd.isBoGo));
							return true;
						}
						else if (cart.HasItemInCart(newItemToAdd.Name ?? string.Empty) != 0) //cart contains the item
						{
							cart.AddOrUpdate(new CartItemByWeight(newItemToAdd.Name ?? String.Empty, newItemToAdd.Description ?? String.Empty, newItemToAdd.Price, amountToAdd + cart.GetCartItemWeight(cart.GetCartItemByID(cart.HasItemInCart(newItemToAdd.Name ?? string.Empty)) as CartItemByWeight ?? new CartItemByWeight()), newItemToAdd.isBoGo, cart.HasItemInCart(newItemToAdd.Name ?? string.Empty)));
							return true;
						}
						else
							return false;
					}
					else return false;
				}
				else
					return false;
            }
		}

		public void AddToCart( InventoryItem item, int quantity = 0, double weight = 0)
        {
			if(item != null)
            {
				if (item is InventoryItemByQuantity)
				{
					CartItemByQuantity newItem = new CartItemByQuantity(item as InventoryItemByQuantity, quantity);
					CartService.Current.AddOrUpdate(newItem);
					decreaseItemQuantity(item as InventoryItemByQuantity, quantity);
				}
				else if (item is InventoryItemByWeight)
				{
					CartItemByWeight newItem = new CartItemByWeight(item as InventoryItemByWeight, weight);
					CartService.Current.AddOrUpdate(newItem);
					decreaseItemWeight(item as InventoryItemByWeight, weight);
				}
			}
			
		}

		public void decreaseItemQuantity(InventoryItemByQuantity item, int quan)
        {
			item.Quantity -= quan;
        }

		public void decreaseItemWeight(InventoryItemByWeight item, double weight)
		{
			item.Weight -= weight;
		}


		public void Load(string fileName = null)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = $"{persistPath}\\InventorySaveData.json";
			}
			else
			{
				fileName = $"{persistPath}\\{fileName}.json";
			}

			var inventoryJson = File.ReadAllText(fileName);
			inventoryList = JsonConvert.DeserializeObject<List<InventoryItem>>
				(inventoryJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
				?? new List<InventoryItem>();

		}

		public void Save(string fileName = null)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = $"{persistPath}\\InventorySaveData.json";
			}
			else
			{
				fileName = $"{persistPath}\\{fileName}.json";
			}
			var inventoryJson = JsonConvert.SerializeObject(inventoryList
				, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
			File.WriteAllText(fileName, inventoryJson);
		}

		public void SetBoGoStatus(InventoryItem item)
        {
			Console.WriteLine("Enter BoGo status: (yes or no)");
			var BoGo = Console.ReadLine() ?? String.Empty;
			item.isBoGo = BoGo == "yes";
        }

		//implement ListNav -- State Full
		private string query;
		private SortType sort = SortType.ID;
		public IEnumerable<InventoryItem> Search(string query)
        {
			this.query = query;
			return ProcessedList;
        }

		public IEnumerable<InventoryItem> ProcessedList
        {
			get
			{
				if (string.IsNullOrEmpty(this.query))
					return Inventory;
				else
				{
					var unorderedList = Inventory
					.Where(i => string.IsNullOrEmpty(this.query) || ((i?.Name?.ToUpper()?.Contains(this.query.ToUpper()) ?? false)
					|| (i?.Description?.ToUpper()?.Contains(this.query.ToUpper()) ?? false))); //search -- filter 
					return OrderList(unorderedList);
				}
			}
        }
		public IEnumerable<InventoryItem> OrderList(IEnumerable<object> list)
		{
			SetSortType();
			var unorderedList = list as IEnumerable<InventoryItem> ?? new List<InventoryItem>();
			if (sort == SortType.Name)
				return unorderedList.OrderBy(i => i.Name);
			else if (sort == SortType.Price)
				return unorderedList.OrderBy(i => i.Price);
			else //sort == SortType.ID
				return unorderedList.OrderBy(i => i.Id);
		}

		public void ListItems(IEnumerable<object> list, int pageSize = 5)
		{
			ListNavigator<object> ListNav = new ListNavigator<object>(list, pageSize);
			string choice;
			ListNav.PrintItems(ListNav.GoToFirstPage());
			do
			{
				if (ListNav.HasNextPage)
					Console.Write("Press d to display NEXT page. ");
				if (ListNav.HasPreviousPage)
					Console.Write("Press a to display PREVIOUS page. ");
				Console.WriteLine("Press s to select SORT type. Press x to EXIT list view.");
				choice = Console.ReadLine() ?? String.Empty;
				if (choice == "s")
				{
					ListNav = new ListNavigator<object>(OrderList(list), pageSize);
					try { ListNav.PrintItems(ListNav.GetCurrentPage()); }
					catch (Exception ex) { ex.GetBaseException(); }
				}
				else if (choice == "d")
					try { ListNav.PrintItems(ListNav.GoForward()); }
					catch (Exception ex) { ex.GetBaseException(); }
				else if (choice == "a")
					try { ListNav.PrintItems(ListNav.GoBackward()); }
					catch (Exception ex) { ex.GetBaseException(); }
				else if (choice == "x")
					break;
				else
				{
					Console.WriteLine("Invalid Choice -- Try Again");
					try { ListNav.PrintItems(ListNav.GetCurrentPage()); }
					catch (Exception ex) { ex.GetBaseException(); }
				}
			} while (choice != "x");
		}
		public void SetSortType()
        {
			Console.WriteLine("How would you like to order the list?");
			Console.WriteLine("1. Sort by name.");
			Console.WriteLine("2. Sort by unit price.");
			Console.WriteLine("3. Sort by ID.");
			var choice = Console.ReadLine() ?? string.Empty;
			switch(choice)
            {
				case "1":
					this.sort = SortType.Name;
					return;
				case "2":
					this.sort = SortType.Price;
					return;
				default:
				{ 
					this.sort = SortType.ID;
					return;
				}
            }

		}

		public enum SortType
        {
			Name, Price, ID
        }
	}
}