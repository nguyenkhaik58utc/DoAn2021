using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class TrainingItem
    {
        public int ID { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Contents { get; set; }
        public System.DateTime Time { get; set; }
        public bool IsShow { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public AttachmentFileItem File { get; set; }
    }
}
