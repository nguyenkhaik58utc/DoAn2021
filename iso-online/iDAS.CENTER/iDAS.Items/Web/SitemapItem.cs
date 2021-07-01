using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SitemapItem
    {
        public int ID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Text { get; set; }
        public string Tooltip { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }
        public string Layout { get; set; }
        public string Script { get; set; }
        public string Tags { get; set; }
        public bool IsPageDynamic { get; set; }
        public bool IsMenuTop { get; set; }
        public bool IsMenuRight { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int Position { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsSelected { get; set; }
        public bool IsParent { get; set; }
    }
}
