using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanTrainingPlanDetailItem
    {
        public int ID { get; set; }
        public int PlanID { get; set; }
        public string Content { get; set; }
        public int Number { get; set; }
        public string Reason { get; set; }
        public bool TrainingTypeID { get; set; }
        public Nullable<decimal> ExpectedCost { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsCommit { get; set; }
        public string CommitContent { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public string Address { get; set; }
        public bool IsCancel { get; set; }
        public string ReasonCancel { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public int ToTalGood { get; set; }
        public int NumberRegister { get; set; }
        public int NumberReal { get; set; }
        public int TotalCreat { get; set; }
        public int TotalAvg { get; set; }
        public int TotalBad { get; set; }
        public int TotalBest { get; set; }
    }
}
