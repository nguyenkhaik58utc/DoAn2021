using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class NewsItem
    {
        public int ID { get; set; }
        public int NewsCategoryID { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Html { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; }
        public string Description { get; set; }
        public bool IsImportal { get; set; }
        public bool IsFirst { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Author { get; set; }
        public Nullable<int> TotalView { get; set; }
        public Nullable<System.DateTime> RefreshAt { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
