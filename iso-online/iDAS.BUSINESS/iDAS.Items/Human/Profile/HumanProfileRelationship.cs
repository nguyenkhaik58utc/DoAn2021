using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Items
{
    public class HumanProfileRelationshipItem
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public Nullable<short> Age { get; set; }
        public Nullable<bool> IsMale { get; set; }
        public string sex { get; set; }
        public string Relationship { get; set; }
        public string Job { get; set; }
        public string PlaceOfJob { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
    }
}
