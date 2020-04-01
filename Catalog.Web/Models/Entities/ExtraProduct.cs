using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class ExtraProduct
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Properties { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public DateTime CreateDate { get; set; }

        public int TotalCount { get; set; }

        public string Pictures { get; set; }
    }
}
