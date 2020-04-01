using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PreCode { get; set; }
        public int StateId { get; set; }
    }
}
