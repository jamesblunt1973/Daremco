using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Web.Data;
using Catalog.Web.Models.Entities;
using Catalog.Web.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catalog.Web
{
    public class PostModel : PageModel
    {
        private readonly IRepository repository;

        [BindProperty(SupportsGet = true)]
        public Posts Id { get; set; }

        public PostModel(IRepository repository)
        {
            this.repository = repository;
        }

        public Post Post { get; set; }

        public async Task OnGet()
        {
            Post = await repository.GetPost(Id);
        }
    }
}