using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Items
{
    public class HumanProfileDiplomaItem
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Major { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Level { get; set; }
        public string FormOfTrainning { get; set; }
        public string Type { get; set; }
        public string Rank { get; set; }
        public string Place { get; set; }
        public string Condition { get; set; }
        public Nullable<System.DateTime> DateOfGraduation { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}
