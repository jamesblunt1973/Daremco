using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class MiscCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MiscGallery> Galleries { get; set; }
    }
}
