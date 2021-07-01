using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ServiceItem
    {
        public int ID { get; set; }
        public int ServiceCategoryID { get; set; }
        public Nullable<int> WebSitemapID { get; set; }
        public SitemapItem Sitemap { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int Position { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
