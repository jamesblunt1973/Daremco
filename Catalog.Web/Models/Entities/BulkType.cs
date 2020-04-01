using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class BulkType : TypeBase
    {
        public byte Count { get; set; }
    }
}
