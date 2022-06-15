using Library.ECommerce.Services;
using eCommerce.Helpers;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string user;
            var productService = ProductService.Current;
            Console.WriteLine("Welcome to the Marconi's Magical Wares!");
            Console.WriteLine("Enter 1 for customer, 2 for employee!");
            user = Console.ReadLine() ?? String.Empty;
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
                    Helpers.ListItems(productService.Inventory);

                }
                else if (action == ActionType.PrintCart)
                {
                    Console.WriteLine("Current Cart");
                    Helpers.ListItems(productService.Cart);
                    Console.Write("\n");
                }
                else if (action == ActionType.Create)
                {
                    productService.AddOrUpdate(Helpers.FillInventoryItem(null));
                }
                else if (action == ActionType.Update)
                {
                    var itemId = SelectInventoryItem("update");
                    var itemToUpdate = productService.Inventory.FirstOrDefault(i => i.Id == itemId);
                    if (itemToUpdate != null)
                    {
                        productService.AddOrUpdate(Helpers.FillInventoryItem(itemToUpdate));
                    }
                }
                else if (action == ActionType.SetBogo )
                {
                    var itemId = SelectInventoryItem("set BoGo status");
                    var itemToUpdate = productService.Inventory.FirstOrDefault(i => i.Id == itemId);
                    if (itemToUpdate != null)
                    {
                        productService.SetBoGoStatus(itemToUpdate);
                    }
                }
                else if (action == ActionType.Delete)
                {
                    var indexToDelete = SelectInventoryItem("delete");
                    productService.Delete(indexToDelete);
                }
                else if (action == ActionType.Save)
                {
                    productService.Save();
                }
                else if (action == ActionType.Load)
                {
                    productService.Load();
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
                    Helpers.ListItems(productService.Inventory);
                    Console.WriteLine("You have chosen to add a product to the cart.");
                    Console.WriteLine("What product would you like to add to cart?");
                    var productId = int.Parse(Console.ReadLine() ?? string.Empty);
                    Console.WriteLine("Enter amount of item to be added to cart.");
                    var amount = decimal.Parse(Console.ReadLine() ?? string.Empty);
                    if (productService.AddToCart(productId, amount))
                        Console.WriteLine("Product added to cart successfully!");
                    else
                        Console.WriteLine("Product failed to add to cart.");
                }
                else if (action == ActionType.DeleteFromCart)
                {
                    var indexToDelete = SelectCartItem("delete");
                    productService.DeleteFromCart(indexToDelete);
                }
                else if (action == ActionType.Checkout)
                {
                    Console.WriteLine("You have chosen to checkout.");
                    productService.Checkout();
                    Console.WriteLine($"Thank you for your business! ByeBye!");
                    cont = false;
                }
                else if (action == ActionType.SearchInv)
                {
                    Console.WriteLine("Please enter your search query:");
                    Helpers.ListItems(productService.GetFilteredList(Console.ReadLine() ?? string.Empty));
                }
                else if (action == ActionType.SearchCart)
                {
                    Console.WriteLine("Please enter your search query:");
                    Helpers.ListItems(productService.GetFilteredListCart(Console.ReadLine() ?? string.Empty));

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
            Console.WriteLine("7. Load Inventory");
            Console.WriteLine("8. Exit");

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
                        return ActionType.Load;
                    case "8":
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
            Console.WriteLine("9. Load Inventory/Cart");
            Console.WriteLine("10. Exit");

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
                        return ActionType.Load;
                    case "10":
                        return ActionType.Exit;
                    default:
                        return ActionType.InvalidChoice;

                }
            }
        }

        private static int SelectInventoryItem(string action)
        {
            Helpers.ListItems(ProductService.Current.Inventory);
            while (true)
            {
                Console.WriteLine($"Which inventory item would you like to {action}?");
                if (int.TryParse(Console.ReadLine(), out var id))
                {
                    return id;
                }
            }

        }

        private static int SelectCartItem(string action)
        {
            Helpers.ListItems(ProductService.Current.Cart);
            while (true)
            {
                Console.WriteLine($"Which inventory item would you like to {action}?");
                if (int.TryParse(Console.ReadLine(), out var id))
                {
                    return id;
                }
            }

        }
    }

    public enum ActionType
    {
        PrintInv, PrintCart, Create, Update,
        Delete, Save, Load, Exit, AddToCart, DeleteFromCart,
        Checkout, SearchInv, SearchCart, InvalidChoice, Dummy, SetBogo
    }

    
}