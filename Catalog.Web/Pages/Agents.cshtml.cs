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
    public class AgentsModel : PageModel
    {
        private readonly IRepository repository;
        public AgentsModel(IRepository repository)
        {
            this.repository = repository;
        }

        [FromQuery]
        public FilterData FilterData { get; set; }

        public IEnumerable<State> States { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Agent> Agents { get; set; }
        public async Task OnGet()
        {
            States = await repository.GetStates();
            if (FilterData.StateId.HasValue)
                Cities = await repository.GetCities(FilterData.StateId.Value);
            Agents = await repository.GetAgents(FilterData);
        }
    }
}