using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Items
{
    public class HumanProfileDisciplineItem
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string NumberOfDecision { get; set; }
        public Nullable<System.DateTime> DateOfDecision { get; set; }
        public string Reason { get; set; }
        public string Form { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}
