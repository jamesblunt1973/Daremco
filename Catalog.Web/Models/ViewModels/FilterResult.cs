using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.ViewModels
{
    public class FilterResult
    {
        public string CategoryName { get; set; }
        public string GalleryName { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<ProductSummury> Products { get; set; }
    }
}
