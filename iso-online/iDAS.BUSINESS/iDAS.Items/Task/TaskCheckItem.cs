using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TaskCheckItem
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public System.DateTime Time { get; set; }
        public decimal Rate { get; set; }
        public bool IsPass { get; set; }
        public HumanEmployeeViewItem Checker { get; set; }
        public int CheckBy { get; set; }
        public string CheckByName { get; set; }
        public string Feedback { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public bool IsComplete { get; set; }
        public System.DateTime CompleteTime { get; set; }   

        public System.DateTime TimePerform { get; set; }
        public HumanEmployeeViewItem Perform { get; set; }
        public decimal RatePerform { get; set; }
        public string PerformByName { get; set; }
        public string FeedbackPerform { get; set; }
        public string ContentPerform { get; set; }
        public string ContentTask { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
    }
}
