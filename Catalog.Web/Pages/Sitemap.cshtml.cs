using Catalog.Web.Data;
using Catalog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Web
{
    public class SitemapModel : PageModel
    {
        private readonly IRepository repo;

        public SitemapModel(IRepository repo)
        {
            this.repo = repo;
        }
        public IEnumerable<SitemapProduct> Products { get; set; }
        public async Task OnGet()
        {
            Products = await repo.GetSiteMapProducts();
        }
    }
}