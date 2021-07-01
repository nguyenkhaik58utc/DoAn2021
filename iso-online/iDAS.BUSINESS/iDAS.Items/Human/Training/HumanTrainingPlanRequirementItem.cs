using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanTrainingPlanRequirementItem
    {
        public int ID { get; set; }
        public int RequirementID { get; set; }
        public int PlanID { get; set; }
        public HumanEmployeeViewItem RequireBy { get; set; }
        public string Content { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}
