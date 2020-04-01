using Catalog.Web.Models.Entities;
using System.Collections.Generic;
using Attribute = Catalog.Web.Models.Entities.Attribute;

namespace Catalog.Web.Models.ViewModels
{
    public class PrimaryData
    {
        public Dictionary<int, DkbRaj> Rajs { get; set; }
        public Dictionary<int, TieLength> TieLengths { get; set; }
        public Dictionary<int, string> Layouts { get; set; }
        public Dictionary<int, string> TieTypes { get; set; }
        public Dictionary<int, SendMethod> SendMethods { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public Dictionary<string, string> Messages { get; set; }
        public Dictionary<int, string> PlanTypes { get; set; }
        public Dictionary<int, Material> Materials { get; set; }
        public Dictionary<int, Attribute> Attributes { get; set; }
        public Dictionary<int, ExtraCategory> ExtraCategories { get; set; }
        public List<Size> Sizes { get; set; }
    }
}
