using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Items
{
    public class HumanRecruitmentPlanTaskItem : TaskItem
    {
        public int PlanID { get; set; }
        public int TaskID { get; set; }
        public decimal Cost { get; set; }
        public bool IsInterview { get; set; }
    }
}