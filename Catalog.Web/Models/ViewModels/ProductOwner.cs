using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.ViewModels
{
    public class ProductOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int MessageLine { get; set; }
    }
}
