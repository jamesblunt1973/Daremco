using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class BulkTypeWeight
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public int BulkWeight { get; set; }
        public int Price { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public int Position { get; set; }
    }
}
