//using eCommerce.UWP.ViewModels;
using Library.eCommerce.Standard.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool isBoGo { get; set; }
        public int Id { get; set; }
        public double AmountToAdd { get; set; }
    }
}
