using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class MiscProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public byte DiscountPercent { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public bool PreSale { get; set; }
        public int TotalCount { get; set; }
    }
}
