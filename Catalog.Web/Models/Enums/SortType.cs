using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Enums
{
    public enum SortType
    {
        [Display(Name = "جدیدترین‌ها")]
        Newest,
        [Display(Name = "پرفروش ترین‌ها")]
        MostSale,
        [Display(Name = "ارزان ترین‌ها")]
        Cheapest,
        [Display(Name = "بیشترین تخفیف‌ها")]
        MostDiscount,
        [Display(Name = "پربازدیدترین‌ها")]
        MostView
    }
}
