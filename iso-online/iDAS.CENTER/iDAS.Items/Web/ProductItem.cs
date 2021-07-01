using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductItem
    {
        public int ID { get; set; }
        public int ProductCategoryID { get; set; }
        public Nullable<int> WebSitemapID { get; set; }
        public SitemapItem Sitemap { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> PublicAt { get; set; }
        public string Version { get; set; }
        public Nullable<int> TotalRegister { get; set; }
        public Nullable<int> Rate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int Position { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
