using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Newtonsoft.Json;
using ListNavigator;

namespace Library.eCommerce.Services
{
    public class CartService
    {
        private List<CartItem> cartList;
        public List<CartItem> Cart
        {
            get { return cartList; }
        }

        private static CartService? current;
        public static CartService Current
        {
            get
            {
                if (current == null)
                    current = new CartService();
                return current;
            }
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

        public decimal GetCartItemWeight(CartItemByWeight item)
        {
            return item.Weight;
        }

        public IEnumerable<CartItem> GetFilteredListCart(string? query) //depricated
        {
            if (string.IsNullOrEmpty(query))
            {
                return Cart;
            }
            return Cart.Where(i =>
                    (i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                    || (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }

        public void DeleteFromCart(int id)
        {
            var itemToDelete = cartList.FirstOrDefault(i => i.Id == id);
            if (itemToDelete != null)
            {
                cartList.Remove(itemToDelete);
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
            var productCartJson = File.ReadAllText("SaveDataCart.json");
            cartList = JsonConvert.DeserializeObject<List<CartItem>>(productCartJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) ?? new List<CartItem>();
        }

        public void Save()
        {
            var productCartJson = JsonConvert.SerializeObject(Cart, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText("SaveDataCart.json", productCartJson);
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
                    SetSortType();
                    var unorderedList = Cart
                    .Where(i => string.IsNullOrEmpty(this.query) || ((i?.Name?.ToUpper()?.Contains(this.query.ToUpper()) ?? false)
                    || (i?.Description?.ToUpper()?.Contains(this.query.ToUpper()) ?? false))); //search -- filter 
                    if (sort == SortType.Name)
                        return unorderedList.OrderBy(i => i.Name);
                    else if (sort == SortType.TotalPrice)
                        return unorderedList.OrderBy(i => i.TotalPrice);
                    else //sort == SortType.ID
                        return unorderedList.OrderBy(i => i.Id);
                }
            }
        }

        public IEnumerable<CartItem> OrderedList
        {
            get
            {
                SetSortType();
                var unorderedList = Cart;
                if (sort == SortType.Name)
                    return unorderedList.OrderBy(i => i.Name);
                else if (sort == SortType.TotalPrice)
                    return unorderedList.OrderBy(i => i.TotalPrice);
                else //sort == SortType.ID
                    return unorderedList.OrderBy(i => i.Id);
            }
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
                    ListNav = new ListNavigator<object>(OrderedList, pageSize);
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
                        Console.WriteLine("Invalid Choice -- Defaulting to order by id.");
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
