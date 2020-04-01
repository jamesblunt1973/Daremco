using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Context { get; set; }
        public int VisitCount { get; set; }
        public double Rate { get; set; }
        public int RateNum { get; set; }
    }
}
