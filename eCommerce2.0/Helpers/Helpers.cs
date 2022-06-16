using System;
using Library.eCommerce.Models;
using ListNavigator;
namespace eCommerce.Helpers
{
	public class Helpers
	{
        internal static void ListItems(IEnumerable<object> list)
        {
            foreach (object item in list)
                Console.WriteLine(item);
        }

        internal static InventoryItem FillInventoryItem(InventoryItem? invItem)
        {
            Console.WriteLine("What is the name of the product?");
            var name = Console.ReadLine();

            Console.WriteLine("What is the description of the product?");
            var desc = Console.ReadLine();

            Console.WriteLine("Is product BoGo? (yes or no)");
            var BoGo = Console.ReadLine() ?? String.Empty;
            while (BoGo != "yes" && BoGo != "no")
            {
                Console.WriteLine("Invalid Entry -- Try Again");
                BoGo = Console.ReadLine() ?? String.Empty;
            }

            Console.WriteLine("Enter 1(ProductByQuantity) or 2(ProductByWeight)");
            var weightOrQuan = Console.ReadLine() ?? String.Empty;
            while (weightOrQuan != "1" && weightOrQuan != "2")
            {
                Console.WriteLine("Invalid Entry -- Try Again");
                weightOrQuan = Console.ReadLine() ?? String.Empty;
            } 

            if (weightOrQuan == "1")
            {
                while (true)
                {
                    Console.WriteLine("What is the price of the product?");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.WriteLine("How many are available?");
                        var quant = 1;
                        if (!int.TryParse(Console.ReadLine(), out quant))
                        {
                            Console.WriteLine("Does not compute -- defaulting quantity to 1");
                        }
                        
                            
                        if (invItem == null)
                        {
                            return new InventoryItemByQuantity(name ?? string.Empty, desc ?? string.Empty, price, quant, (BoGo == "yes"));
                        }
                        var newInvItem = invItem as InventoryItemByQuantity ?? new InventoryItemByQuantity();
                        newInvItem.Name = name ?? string.Empty;
                        newInvItem.Description = desc ?? string.Empty;
                        newInvItem.Price = price;
                        newInvItem.Quantity = quant;
                        newInvItem.isBoGo = (BoGo == "yes");
                        return newInvItem;
                    }
                }
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("What is the price of the product?");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.WriteLine("How many pounds are available?");
                        decimal weight = 1;
                        if (!decimal.TryParse(Console.ReadLine(), out weight))
                        {
                            Console.WriteLine("Does not compute -- defaulting quantity to 1");
                        }
                        if (invItem == null)
                        {
                            return new InventoryItemByWeight(name ?? string.Empty, desc ?? string.Empty, price, weight, (BoGo == "yes"));
                        }
                        var newInvItem = invItem as InventoryItemByWeight ?? new InventoryItemByWeight();
                        newInvItem.Name = name ?? string.Empty;
                        newInvItem.Description = desc ?? string.Empty;
                        newInvItem.Price = price;
                        newInvItem.Weight = weight;
                        newInvItem.isBoGo = (BoGo == "yes");
                        return newInvItem;
                    }
                }
            }
        }
    }
}

