using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperApi.Models
{
    public class CombinedTables
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    }
}
