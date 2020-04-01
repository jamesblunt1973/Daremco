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
    public class ExtraProductModel : PageModel
    {
        private readonly IRepository repository;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public ExtraProduct Product { get; set; }

        public ExtraProductModel(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task OnGet()
        {
            Product = await repository.GetExtraProduct(Id);
        }
    }
}