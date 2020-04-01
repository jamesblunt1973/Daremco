using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class ProductColor
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? MaterialGroupId { get; set; }
        public int? PaletteColorId { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
    }
}
