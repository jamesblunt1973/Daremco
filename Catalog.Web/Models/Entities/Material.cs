using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public double PriceWeightCoef { get; set; }
        public double ShowWeightCoef { get; set; }
        public double ColoringCoef { get; set; }
        public string Description { get; set; }
        public string Sign { get; set; }
        public int Materials { get; set; }
        public bool BulkSales { get; set; }
        public int BulkMinWeight { get; set; }
        public int BulkMinPrice { get; set; }
        public int BulkMaxWeight { get; set; }
        public int BulkMaxPrice { get; set; }
        public bool InStock { get; set; }
        public string Turns { get; set; }
    }
}
