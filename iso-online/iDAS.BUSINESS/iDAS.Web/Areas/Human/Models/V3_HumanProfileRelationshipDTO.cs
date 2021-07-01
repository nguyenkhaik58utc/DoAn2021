using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class V3_HumanProfileRelationshipDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }
        public int RelationshipID { get; set; }
        public string Job { get; set; }
        public string PlaceOfJob { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsSelf { get; set; }
    }
    public class V3_HumanProfileRelationshipResponse
    {
        public int ID { get; set; }
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
