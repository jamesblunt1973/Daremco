using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class TieType
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public bool VariableLength { get; set; }
    }
}
