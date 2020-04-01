using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Models.Entities
{
    public class Agent
    {
        public int Id { get; set; }
        public string Address { get; set; } 
        public string AgentStatus { get; set; }
        public string AgentsWorkHours { get; set; }
        public string Bio { get; set; }
        public string Cell { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
		public bool? Gender { get; set; }
        public string UserName { get; set; }
        public string Tell { get; set; } 
        public string Name { get; set; } 
        public string MapLink { get; set; }
        public string IntroLink { get; set; } 
        public string State { get; set; } 
        public string City { get; set; } 
        public int RoleId { get; set; }
        public string Email { get; set; }
        public int TotalCount { get; set; }
    }
}
