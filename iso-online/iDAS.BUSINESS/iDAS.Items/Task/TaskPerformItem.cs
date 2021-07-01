using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TaskPerformItem
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public System.DateTime Time { get; set; }
        public HumanEmployeeViewItem Perform { get; set; }
        public decimal Rate { get; set; }
        public Nullable<int> PerformBy { get; set; }
        public string PerformByName { get; set; }
        public string Feedback { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public bool IsCheck { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
        public string ContentTask { get; set; }
    }
}
