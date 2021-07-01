using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DepartmentLegalItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUse { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public int? ISOID { get; set; }
        public HumanDepartmentViewItem DepartmentApply { get; set; }
        public bool IsApplyAll { get; set; }
        public bool IsRoot { get; set; }
    }
}
