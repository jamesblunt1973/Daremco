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
    public class MiscModel : PageModel
    {
        private readonly IRepository repository;
        public MiscModel(IRepository repository)
        {
            this.repository = repository;
        }

        [FromQuery]
        public FilterData FilterData { get; set; }

        public IEnumerable<MiscCategory> Categories { get; set; }
        public IEnumerable<MiscProduct> Products { get; set; }
        public async Task OnGet()
        {
            Categories = await repository.GetMiscCategories();
            Products = await repository.GetMiscProducts(FilterData);
        }
    }
}