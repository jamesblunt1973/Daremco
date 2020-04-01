using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int PlanTypeId { get; set; }
        public int Price { get; set; }
        public int Parameter { get; set; }
    }
}
