using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Web.Data;
using Catalog.Web.Models.Entities;
using Catalog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catalog.Web.Pages
{
    public class ExtraModel : PageModel
    {
        private readonly IRepository repository;

        [FromQuery]
        public FilterData FilterData { get; set; }
        public IEnumerable<ExtraProduct> Products { get; set; }
        public PrimaryData PrimaryData { get; set; }

        public ExtraModel(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task OnGet()
        {
            if (FilterData.CategoryId.HasValue)
            {
                PrimaryData = await repository.GetPrimaryData();
                Products = await repository.GetExtraProducts(FilterData);
            }
        }
    }
}