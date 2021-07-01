using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductModuleItem
    {
        public int ID { get; set; }
        public int WebProductID { get; set; }
        public int CenterModuleID { get; set; }
        public string CenterModuleName { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; } 
        public string Tags { get; set; }
        public string Html { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Description { get; set; }
        public int Position { get; set; } 
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsSelected { get; set; }
    }
}
