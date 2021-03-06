using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DepartmentRequirmentItem
    {
        public int ID { get; set; }
        public int CenterDepartmentRequirmentCategoryID { get; set; }
        public string RequirmentCategoryName { get; set; }
        public bool IsInner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUse { get; set; }
        public string Content { get; set; }
        public string Scope { get; set; }
        public string Object { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public int ISOStandardID { get; set; }

        public string ISOName { get; set; }
    }
}
