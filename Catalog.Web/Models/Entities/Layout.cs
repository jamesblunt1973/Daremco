using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class Layout
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}
