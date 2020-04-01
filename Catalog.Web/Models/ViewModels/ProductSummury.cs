using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.ViewModels
{
    public class ProductSummury
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Moghat { get; set; }
        public int TiesWidth { get; set; }
        public int TiesHeight { get; set; }
        public string DeliveryMessage { get; set; }
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }
        public int Stock { get; set; }
        public int ColorsCount { get; set; }
        public bool Skein { get; set; }
        public byte Discount { get; set; }
        public int TypeCounts { get; set; }

        public string CategoryName { get; set; }
        public string GalleryName { get; set; }
        public int TotalCount { get; set; }
    }
}
