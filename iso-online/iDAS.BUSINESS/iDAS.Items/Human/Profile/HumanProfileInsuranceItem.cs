using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;


namespace iDAS.Items
{
    public class HumanProfileInsuranceItem
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Type { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public System.DateTime? UpdateAt { get; set; }
        public string Note { get; set; }
        public int EmployeeID { get; set; }
        public Nullable<decimal> InSuranceNorms { get; set; }
    }
}
