using System;
using Catalog.Web.Models.Enums;

namespace Catalog.Web.Models.ViewModels
{
    public class FilterData
    {
        [QueryParam]
        public bool? InStock { get; set; }

        [QueryParam]
        public bool? InProduction { get; set; }

        [QueryParam]
        public int? Page { get; set; }

        [QueryParam]
        public int? Count { get; set; }

        [QueryParam]
        public int? Id { get; set; }

        [QueryParam]
        public string Name { get; set; }

        [QueryParam]
        public int? CategoryId { get; set; }

        [QueryParam]
        public int? GalleryId { get; set; }

        [QueryParam]
        public SortType? Sort { get; set; }

        [QueryParam]
        public int? Raj { get; set; }

        [QueryParam]
        public int? LayoutId { get; set; }

        [QueryParam]
        public int? TieTypeId { get; set; }

        [QueryParam]
        public string Size { get; set; }

        [QueryParam]
        public Materials? Materials { get; set; }

        [QueryParam]
        public Delivery? Delivery { get; set; }

        [QueryParam]
        public string TiesWidth { get; set; }

        [QueryParam]
        public string TiesHeight { get; set; }

        [QueryParam]
        public string DimsWidth { get; set; }

        [QueryParam]
        public string DimsHeight { get; set; }

        [QueryParam]
        public string Moghat { get; set; }

        [QueryParam]
        public string Colors { get; set; }

        [QueryParam]
        public bool? Discount { get; set; }

        [QueryParam]
        public bool? FreePlans { get; set; }

        [QueryParam]
        public string Attributes { get; set; }

        [QueryParam]
        public int? StateId { get; set; }

        [QueryParam]
        public int? CityId { get; set; }

        // فیلدهایی که نباید به پارامتر تبدیل شوند
        public ViewType? ViewType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class QueryParamAttribute : Attribute
    {
    }
}
