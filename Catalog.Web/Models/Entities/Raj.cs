using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    [Table("Rajs")]
    public class DkbRaj
    {
        public int Raj { get; set; }
        public double CostCoef { get; set; }
        public int? MainRaj { get; set; }
        public List<MaterialRaj> MaterialRajs { get; set; }
    }

    public class MaterialRaj
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
    }
}
