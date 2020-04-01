using Catalog.Web.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class TypeBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Raj { get; set; }
        public int DimsWidth { get; set; }
        public int DimsHeight { get; set; }
        public Materials Materials { get; set; }
        public int? MaterialId { get; set; }
        public int MainTieLengthId { get; set; }
        public int TieTypeId { get; set; }
        public int Price { get; set; }
        public bool Visible { get; set; }
        public byte Size { get; set; }
    }
}
