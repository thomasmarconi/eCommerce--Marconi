using System;
using Library.eCommerce.Models;
namespace eCommerce.Helpers
{
	public class Helpers
	{
        internal static void ListItems(IEnumerable<object> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        internal static InventoryItem FillInventoryItem(InventoryItem? invItem)
        {
            Console.WriteLine("What is the name of the product?");
            var name = Console.ReadLine();

            Console.WriteLine("What is the description of the product?");
            var desc = Console.ReadLine();

            Console.WriteLine("Enter product type: \"weight\" or \"quantity\"");
            var weightOrQuan = Console.ReadLine();

            if (weightOrQuan == "quantity")
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
                            return new InventoryItemByQuantity
                            {
                                Name = name ?? string.Empty
                                ,
                                Description = desc ?? string.Empty
                                ,
                                Price = price
                                ,
                                Quantity = quant
                            };
                        }

                        return new InventoryItemByQuantity(name ?? String.Empty, desc ?? String.Empty, price, quant, invItem.Id);
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
                            return new InventoryItemByWeight
                            {
                                Name = name ?? string.Empty
                                ,
                                Description = desc ?? string.Empty
                                ,
                                Price = price
                                ,
                                Weight = weight
                            };
                        }

                        return new InventoryItemByWeight(name ?? string.Empty, desc ?? string.Empty, price, weight, invItem.Id);
                    }
                }
            }
        }
    }
}

