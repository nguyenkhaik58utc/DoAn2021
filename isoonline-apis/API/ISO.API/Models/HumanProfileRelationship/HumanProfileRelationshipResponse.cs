using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileRelationship
{
    public class HumanProfileRelationshipResponse
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }
        public string Relationship { get; set; }
        public string Job { get; set; }
        public string PlaceOfJob { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsSelf { get; set; }
    }
}
