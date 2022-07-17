using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Newtonsoft.Json;
using ListNavigator;
using System.IO;
using System.Collections.ObjectModel;

namespace Library.eCommerce.Services
{
    public class CartService
    {

        private string persistPath
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";

        private List<CartItem> cartList;
        public List<CartItem> Cart
        {
            get { return cartList; }
        }

        private ObservableCollection<String> savedCartNames;
        public ObservableCollection<String> SavedCartNames
        {
            get
            {
                if (savedCartNames == null)
                    savedCartNames = new ObservableCollection<String>();
                return savedCartNames;
            }
        }
        private String mostRecentCart;

        private static CartService current;
        public static CartService Current
        {
            get
            {
                if (current == null)
                    current = new CartService();
                return current;
            }
        }

        public double Subtotal()
        {
            double Sub = 0;
           
            foreach (CartItem item in Cart)
                Sub += item.TotalPrice;
            return Sub;
        }

        public double Tax()
        {
            return .07 * Subtotal();
        }

        public double Total()
        {
            return Tax() + Subtotal();
        }

        private CartService()
        {
            cartList = new List<CartItem>();
            query = String.Empty;
        }

        public void AddOrUpdate(CartItem item)
        {
            //Id management for adding a new record.
            if (item.Id == 0)
            {
                if (cartList.Any())
                {
                    item.Id = cartList.Select(i => i.Id).Max() + 1;
                }
                else
                {
                    item.Id = 1;
                }
            }

            if (!cartList.Any(i => i.Id == item.Id))
            {
                cartList.Add(item);
            }
        }

        public int SelectCartItem(string action)
        {
            ListItems(Cart);
            while (true)
            {
                Console.WriteLine($"Which cart item would you like to {action}?");
                if (int.TryParse(Console.ReadLine(), out var id))
                {
                    return id;
                }
            }

        }

        public CartItem GetCartItemByID(int iD)
        {
            return Cart.FirstOrDefault(i =>
                    (i?.Id.Equals(iD) ?? false)) ?? new CartItem();
        }

        public int HasItemInCart(string nameToCheck) //returns item's iD if its in the cart
        {
            foreach(var item in Cart)
            {
                if (item.Name == nameToCheck)
                    return item.Id;
            }
            return 0;
        }

        public int GetCartItemQuantity(CartItemByQuantity item)
        {
            return item.Quantity;
        }

        public double GetCartItemWeight(CartItemByWeight item)
        {
            return item.Weight;
        }

        public IEnumerable<CartItem> GetFilteredListCart(string query) //depricated
        {
            if (string.IsNullOrEmpty(query))
            {
                return Cart;
            }
            return Cart.Where(i =>
                    (i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                    || (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }

        public void Delete(int id)
        {
            var itemToDelete = cartList.FirstOrDefault(i => i.Id == id);
            if (itemToDelete != null)
            {
                cartList.Remove(itemToDelete);
            }
        }

        public void Checkout()
        {
            double subtotal = 0;
            foreach (var product in Cart)
            {
                subtotal += product.TotalPrice;
            }
            var taxAmount = subtotal * (double).07;
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Subtotal is: {Math.Round(subtotal, 2, MidpointRounding.AwayFromZero)}");
            Console.WriteLine($"Tax is: {Math.Round(taxAmount,2,MidpointRounding.AwayFromZero)}");
            Console.WriteLine($"Total is: {Math.Round(subtotal + taxAmount,2,MidpointRounding.AwayFromZero)}\n");
            PaymentMethod();
        }

        public void Checkout(String filename = null)
        {
            SavedCartNames.Remove(mostRecentCart);
            Save();
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
        public void LoadCartNames()
        {
            var cartNamesJson = File.ReadAllText($"{persistPath}\\CartNamesSaveData.json");
            savedCartNames = JsonConvert.DeserializeObject<ObservableCollection<String>>(cartNamesJson);
        }
        public void Load(string fileName = null)
        {
            mostRecentCart = fileName;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\CartSaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json"; 
            }
            var cartJson = File.ReadAllText(fileName);
            cartList = JsonConvert.DeserializeObject<List<CartItem>>
                (cartJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<CartItem>();
            

        }

        public void Save(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\CartSaveData.json";
            }
            else
            {
                SavedCartNames.Add(fileName);
                fileName = $"{persistPath}\\{fileName}.json";
                
            }
            var cartJson = JsonConvert.SerializeObject(cartList
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var cartNamesJson = JsonConvert.SerializeObject(savedCartNames);
            File.WriteAllText($"{persistPath}\\CartNamesSaveData.json", cartNamesJson);
            File.WriteAllText(fileName, cartJson);
        }

        //implement ListNav -- State Full
        private string query;
        private SortType sort = SortType.ID;
        public IEnumerable<CartItem> Search(string query)
        {
            this.query = query;
            return ProcessedList;
        }

        public IEnumerable<CartItem> ProcessedList
        {
            get
            {
                if (string.IsNullOrEmpty(this.query))
                    return Cart;
                else
                {
                    var unorderedList = Cart
                    .Where(i => string.IsNullOrEmpty(this.query) || ((i?.Name?.ToUpper()?.Contains(this.query.ToUpper()) ?? false)
                    || (i?.Description?.ToUpper()?.Contains(this.query.ToUpper()) ?? false))); //search -- filter 
                    return OrderList(unorderedList);
                }
            }
        }

        public IEnumerable<CartItem> OrderList(IEnumerable<object> list)
        {
            SetSortType();
            var unorderedList = list as IEnumerable<CartItem> ?? new List<CartItem>();
            if (sort == SortType.Name)
                return unorderedList.OrderBy(i => i.Name);
            else if (sort == SortType.TotalPrice)
                return unorderedList.OrderBy(i => i.TotalPrice);
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
            Console.WriteLine("How would you like to order the cart?");
            Console.WriteLine("1. Sort by name.");
            Console.WriteLine("2. Sort by total price.");
            Console.WriteLine("3. Sort by ID.");
            var choice = Console.ReadLine() ?? string.Empty;
            switch (choice)
            {
                case "1":
                    this.sort = SortType.Name;
                    return;
                case "2":
                    this.sort = SortType.TotalPrice;
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
            Name, TotalPrice, ID
        }
    }
}
