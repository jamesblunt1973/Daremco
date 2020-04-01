using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class Palette
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool BulkSales { get; set; }
        public bool FixTurn { get; set; }
        public string DeliverySchedule { get; set; }
        public string DeliveryMessage { get; set; }
    }
}
