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
    public class ProductModel : PageModel
    {
        private readonly IRepository repository;

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }
        public Product Product { get; set; }
        public PrimaryData PrimaryData { get; set; }

        public ProductModel(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task OnGet()
        {
            if (Id.HasValue)
            {
                PrimaryData = await repository.GetPrimaryData();
                Product = await repository.GetProduct(Id.Value);
            }
        }
    }
}