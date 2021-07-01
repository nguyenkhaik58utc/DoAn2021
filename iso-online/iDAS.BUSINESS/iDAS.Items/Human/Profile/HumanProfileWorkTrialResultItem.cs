using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanProfileWorkTrialResultItem
    {
        public int ID { get; set; }
        public int HumanProfileWorkTrialID { get; set; }
        public int QualityCriteriaID { get; set; }
        public Nullable<int> EmployeePoint { get; set; }
        public Nullable<int> ManagerPoint { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Note { get; set; }

        public virtual HumanProfileWorkTrialItem HumanProfileWorkTrial { get; set; }
        public virtual QualityCriteriaItem QualityCriteria { get; set; }

        public string QualityCriteriaName { get; set; }

        public string QualityCriteriaCateName { get; set; }
    }
}
