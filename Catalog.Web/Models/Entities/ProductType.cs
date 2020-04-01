using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class ProductType : TypeBase
    {
        public int Stock { get; set; }
        public bool Continued { get; set; }
        public int? PositiveSale { get; set; }
        public string DeliveryMessage { get; set; }
        public string AttributeIds { get; set; }
    }
}
