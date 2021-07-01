using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class EquipmentProductionMaintainItem
    {
        public int ID { get; set; }
        public int EquipmentProductionID { get; set; }
        public HumanDepartmentViewItem HumanDepartment { get; set; }
        public Nullable<System.DateTime> PlanTime { get; set; }
        public Nullable<System.DateTime> RealTime { get; set; }
        public int Number { get; set; }
        public int Year
        {
            get
            {
                if (PlanTime.HasValue)
                {
                    return PlanTime.Value.Year;
                }
                return 0;
            }
        }
        public string Status
        {
            get
            {
                if (PlanTime.HasValue && RealTime.HasValue)
                {
                    if (RealTime.Value <= PlanTime.Value)
                    {
                        return "Đạt";
                    }
                    else return "Không đạt";
                }
                return "";
            }
        }
        public bool IsComplete { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
    }
}
