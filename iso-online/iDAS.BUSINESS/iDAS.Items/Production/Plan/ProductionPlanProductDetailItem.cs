using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionPlanProductDetailItem
    {
        public int ID { get; set; }
        public int ProductionPlanProductID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Month { get { return Date.HasValue ? Date.Value.Month.ToString() : ""; } }
        public HumanDepartmentViewItem HumanDepartment { get; set; }
        public string Level { get; set; }
        public string Color { get; set; }
        public string RequirementName { get; set; }
        public int EmployeeCount { get; set; }
        public int Quantity { get; set; }
        public Nullable<float> Menday { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public int CalculatorQuantity { get; set; }

        public float? CalculatorMenday { get; set; }
    }
}
