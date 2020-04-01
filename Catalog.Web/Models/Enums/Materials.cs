using System.ComponentModel.DataAnnotations;

namespace Catalog.Web.Models.Enums
{
    public enum Materials
    {
        [Display(Name = "تمام پشم")]
        Wool = 1,
        [Display(Name = "پشم و ابریشم")]
        WoolAndSilk,
        [Display(Name = "تمام ابریشم")]
        Silk
    }
}
