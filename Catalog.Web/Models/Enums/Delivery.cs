using System.ComponentModel.DataAnnotations;

namespace Catalog.Web.Models.Enums
{
    public enum Delivery
    {
        [Display(Name = "کلافی")]
        Skein = 1,
        [Display(Name = "بسته‌ای")]
        Bulk
    }
}
