using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperApi.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
