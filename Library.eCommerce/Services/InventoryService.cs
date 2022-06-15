using System;
using Library.eCommerce.Models;
using Newtonsoft.Json;

namespace Library.ECommerce.Services
{
	public class InventoryService
	{
		//data
		private List<InventoryItem> inventoryList;
		public List<InventoryItem> Inventory
		{
			get { return inventoryList; }
		}

		

		private static InventoryService? current;
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

		public bool AddToCart(int productId, decimal amount)
		{
			var itemToAdd = GetInventoryItemByID(productId);
			if (itemToAdd != null)
			{

			}
		}

		public void LoadInventory()
		{
			var productInvJson = File.ReadAllText("SaveDataInv.json");
			inventoryList = JsonConvert.DeserializeObject<List<InventoryItem>>(productInvJson, 
				new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) ?? 
				new List<InventoryItem>();
		}

		public void SaveInventory()
		{
			var productInvJson = JsonConvert.SerializeObject(Inventory, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
			File.WriteAllText("SaveDataInv.json", productInvJson);
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
					if (sort == SortType.Name)
						return unorderedList.OrderBy(i => i.Name);
					else if (sort == SortType.Price)
						return unorderedList.OrderBy(i => i.Price);
					else //sort == SortType.ID
						return unorderedList.OrderBy(i => i.Id);
				}
			}
        }
		
		public void setSortType()
        {
			Console.WriteLine("How would you like to order the list?");
			Console.WriteLine("1. Sort by name.");
			Console.WriteLine("2. Sort by unit price.");
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
					Console.WriteLine("Invalid Choice -- Defaulting to order by id.");
					this.sort = SortType.ID;
					return;
            }

		}

		public enum SortType
        {
			Name, Price, ID
        }
	}
}