using Catalog.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int GalleryId { get; set; }
        public string GalleryName { get; set; }
        public int? PaletteId { get; set; }
        public int LayoutId { get; set; }
        public bool Skein { get; set; }
        public int TiesWidth { get; set; }
        public int TiesHeight { get; set; }
        public double Moghat { get; set; }
        public short ColorsCount { get; set; }
        public DateTime CreateDate { get; set; }
        public double Rate { get; set; }
        public int RateNum { get; set; }
        public bool Visible { get; set; }
        public string ShowDescription { get; set; }
        public byte Discount { get; set; }
        public bool FreePlans { get; set; }
        public IEnumerable<ProductType> ProductTypes { get; set; }
        public IEnumerable<BulkType> BulkTypes { get; set; }
        public IEnumerable<Plan> Plans { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        public int? OwnerId { get; set; }
        public byte OwnerMessageLine { get; set; }
        public string OwnerName { get; set; }
        public string OwnerUserName { get; set; }
        public string DeliveryMessage { get; set; }
    }
}
