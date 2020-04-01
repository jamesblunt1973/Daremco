using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class ExtraCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
    }
}
