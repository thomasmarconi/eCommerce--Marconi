﻿using Library.eCommerce.Services;
using eCommerce.Helpers;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inventoryService = InventoryService.Current;
            var cartService = CartService.Current;
            Console.WriteLine("Welcome to the Marconi's Magical Wares!");
            Console.WriteLine("Would you like to load saved cart/inventory data? (yes or no)");
            var willLoad = Console.ReadLine() ?? String.Empty;
            while(willLoad != "yes" && willLoad != "no")
            {
                Console.WriteLine("Invalid Entry -- Try Again (yes or no)");
                willLoad = Console.ReadLine() ?? String.Empty;
            }
            if(willLoad == "yes")
            {
                inventoryService.Load();
                cartService.Load();
            }
            Console.WriteLine("User Verification: Enter 1 for customer, 2 for employee!");
            var user = Console.ReadLine() ?? String.Empty;
            ActionType action;
            bool cont = true;
            while (cont)
            {   
                if (user == "1")
                    action = CustomerPrintMenu();
                else if (user == "2")
                    action = EmployeePrintMenu();
                else
                {
                    Console.WriteLine("Incorrect User Identification. Please enter valid value.");
                    Console.WriteLine("Enter 1 for customer, 2 for employee!");
                    user = Console.ReadLine() ?? "0";
                    action = ActionType.Dummy;   //dummy assignment
                }

                if (action == ActionType.PrintInv)
                {
                    Console.WriteLine("Current Inventory");
                    inventoryService.ListItems(inventoryService.Inventory);

                }
                else if (action == ActionType.PrintCart)
                {
                    Console.WriteLine("Current Cart");
                    cartService.ListItems(cartService.Cart);
                    Console.Write("\n");
                }
                else if (action == ActionType.Create)
                {
                    inventoryService.AddOrUpdate(Helpers.FillInventoryItem(null));
                }
                else if (action == ActionType.Update)
                {
                    var itemId = inventoryService.SelectInventoryItem("update");
                    var itemToUpdate = inventoryService.Inventory.FirstOrDefault(i => i.Id == itemId);
                    if (itemToUpdate != null)
                    {
                        inventoryService.AddOrUpdate(Helpers.FillInventoryItem(itemToUpdate));
                    }
                }
                else if (action == ActionType.SetBogo )
                {
                    var itemId = inventoryService.SelectInventoryItem("set BoGo status");
                    var itemToUpdate = inventoryService.Inventory.FirstOrDefault(i => i.Id == itemId);
                    if (itemToUpdate != null)
                    {
                        inventoryService.SetBoGoStatus(itemToUpdate);
                    }
                }
                else if (action == ActionType.Delete)
                {
                    var indexToDelete = inventoryService.SelectInventoryItem("delete");
                    inventoryService.Delete(indexToDelete);
                }
                else if (action == ActionType.Save)
                {
                    inventoryService.Save();
                    cartService.Save();
                }
                else if (action == ActionType.Exit)
                {
                    Console.WriteLine("You have chosen to exit. ByeBye!");
                    cont = false;
                }
                else if (action == ActionType.AddToCart)
                {
                    //need to determine whether the item that is wanting to be added is a byWeight or byQuantity
                    Console.WriteLine("Current Inventory");
                    inventoryService.ListItems(inventoryService.Inventory);
                    Console.WriteLine("You have chosen to add a product to the cart.");
                    Console.WriteLine("What product would you like to add to cart?");
                    var productId = int.Parse(Console.ReadLine() ?? string.Empty);
                    if (inventoryService.AddToCart(productId,cartService))
                        Console.WriteLine("Product added to cart successfully!");
                    else
                        Console.WriteLine("Product failed to add to cart.");
                }
                else if (action == ActionType.DeleteFromCart)
                {
                    var indexToDelete = cartService.SelectCartItem("delete");
                    cartService.DeleteFromCart(indexToDelete);
                }
                else if (action == ActionType.Checkout)
                {
                    Console.WriteLine("You have chosen to checkout.");
                    cartService.Checkout();
                    Console.WriteLine($"Thank you for your business! ByeBye!");
                    cont = false;
                }
                else if (action == ActionType.SearchInv)
                {
                    Console.WriteLine("Please enter your search query:");
                    inventoryService.ListItems(inventoryService.Search(Console.ReadLine() ?? string.Empty));
                }
                else if (action == ActionType.SearchCart)
                {
                    Console.WriteLine("Please enter your search query:");
                    cartService.ListItems(cartService.Search(Console.ReadLine() ?? string.Empty));

                }
                else if (action == ActionType.InvalidChoice)
                {
                    Console.WriteLine("Invalid Menu Choice. Please try again.");
                }
            }



        }

        public static ActionType EmployeePrintMenu()
        {
            //CRUD = Create, Read, Update, and Delete
            Console.WriteLine("Select an option to begin:");
            Console.WriteLine("1. List Inventory");
            Console.WriteLine("2. Add New Product to Inventory");
            Console.WriteLine("3. Update a Product");
            Console.WriteLine("4. Set a Product's BoGo status");
            Console.WriteLine("5. Delete a Product");
            Console.WriteLine("6. Save Inventory");
            Console.WriteLine("7. Exit");

            string input = Console.ReadLine() ?? "0";

            while (true)
            {
                switch (input)
                {
                    case "1":
                        return ActionType.PrintInv;
                    case "2":
                        return ActionType.Create;
                    case "3":
                        return ActionType.Update;
                    case "4":
                        return ActionType.SetBogo;
                    case "5":
                        return ActionType.Delete;
                    case "6":
                        return ActionType.Save;
                    case "7":
                        return ActionType.Exit;
                    default:
                        return ActionType.InvalidChoice;

                }
            }
        }
        public static ActionType CustomerPrintMenu()
        {
            //CRUD = Create, Read, Update, and Delete
            Console.WriteLine("Select an option to begin:");
            Console.WriteLine("1. List Inventory");
            Console.WriteLine("2. Show Cart");
            Console.WriteLine("3. Add a Product to Cart");
            Console.WriteLine("4. Delete a Product from Cart");
            Console.WriteLine("5. Checkout");
            Console.WriteLine("6. Search Inventory");
            Console.WriteLine("7. Search Cart");
            Console.WriteLine("8. Save Inventory/Cart");
            Console.WriteLine("9. Exit");

            string input = Console.ReadLine() ?? String.Empty;

            while (true)
            {
                switch (input)
                {
                    case "1":
                        return ActionType.PrintInv;
                    case "2":
                        return ActionType.PrintCart;
                    case "3":
                        return ActionType.AddToCart;
                    case "4":
                        return ActionType.DeleteFromCart;
                    case "5":
                        return ActionType.Checkout;
                    case "6":
                        return ActionType.SearchInv;
                    case "7":
                        return ActionType.SearchCart;
                    case "8":
                        return ActionType.Save;
                    case "9":
                        return ActionType.Exit;
                    default:
                        return ActionType.InvalidChoice;

                }
            }
        }

        

        
    }

    public enum ActionType
    {
        PrintInv, PrintCart, Create, Update,
        Delete, Save, Load, Exit, AddToCart, DeleteFromCart,
        Checkout, SearchInv, SearchCart, InvalidChoice, Dummy, SetBogo, SetSortType
    }

    
}