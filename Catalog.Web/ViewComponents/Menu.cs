using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Web.Data;
using Catalog.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Web.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IRepository repository;

        public MenuViewComponent(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var primaryData = await repository.GetPrimaryData();
            return View(primaryData.ExtraCategories);
        }
    }
}
