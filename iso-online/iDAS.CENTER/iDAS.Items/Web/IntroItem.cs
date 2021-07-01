using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class IntroItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public AttachmentFileItem File { get; set; }
        public bool IsShow { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public int Order { get; set; }
    }
}
