using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class TieLength
    {
        public int Id { get; set; }
        public int TieTypeId { get; set; }
        public double Length { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
    }
}
