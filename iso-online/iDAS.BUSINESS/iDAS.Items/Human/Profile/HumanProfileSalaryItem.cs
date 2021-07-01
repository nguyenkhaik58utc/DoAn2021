using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;


namespace iDAS.Items
{
    public class HumanProfileSalaryItem
    {
        public int ID { get; set; }
        public string Level { get; set; }
        public string Wage { get; set; }
        public Nullable<System.DateTime> DateOfApp { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public System.DateTime? UpdateAt { get; set; }
        public int EmployeeID { get; set; }
        public string Note { get; set; }
    }
}
