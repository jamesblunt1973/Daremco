using Catalog.Web.Data;
using Catalog.Web.Models.Entities;
using Catalog.Web.Models.Enums;
using Catalog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository repository;
        public IndexModel(IRepository repository)
        {
            this.repository = repository;
            PictureSize = 160;
        }

        [FromQuery]
        public FilterData FilterData { get; set; }
        public FilterResult Data { get; set; }
        public int PictureSize { get; set; }
        public PrimaryData PrimaryData { get; set; }

        public async Task OnGet()
        {
            if (FilterData.Count.HasValue)
                Response.Cookies.Append("products-count", FilterData.Count.ToString(), new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                });
            else if (Request.Cookies.ContainsKey("products-count"))
                FilterData.Count = Convert.ToInt32(Request.Cookies["products-count"]);

            if (FilterData.ViewType.HasValue)
            {
                Response.Cookies.Append("view-type", FilterData.ViewType.ToString(), new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                });
            }
            else if (Request.Cookies.ContainsKey("view-type"))
                FilterData.ViewType = Enum.Parse<ViewType>(Request.Cookies["view-type"]);
            else
                FilterData.ViewType = ViewType.ListView; // Default value

            if (FilterData.Sort.HasValue)
            {
                Response.Cookies.Append("sort", FilterData.Sort.ToString(), new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                });
            }
            else if (Request.Cookies.ContainsKey("sort"))
                FilterData.Sort = Enum.Parse<SortType>(Request.Cookies["sort"]);

            if (Request.Cookies.ContainsKey("picture-size"))
            {
                if (int.TryParse(Request.Cookies["picture-size"], out int pictureSize))
                    PictureSize = pictureSize;
                else
                    Response.Cookies.Delete("picture-size");
            }

            PrimaryData = await repository.GetPrimaryData();

            Data = await repository.GetProducts(FilterData);
        }
    }
}
