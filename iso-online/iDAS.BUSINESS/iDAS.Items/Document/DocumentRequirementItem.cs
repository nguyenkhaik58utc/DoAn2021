using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DocumentRequirementItem : ApprovalItem
    {
        public int ID { get; set; }
        public int? DocumentID { get; set; }
        public string Document { get; set; }
        public string Version { get; set; }
        public string Times { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public bool Type { get; set; }
        public string StrType { get; set; }
        public int? ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveAt { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public string Content { get; set; }//ND Yêu cầu
        public string ContentOld { get; set; }//ND Yêu cầu
        public string ContentChange { get; set; }//ND Yêu cầu
        public string ReasonChange { get; set; }//ND Yêu cầu
        public string Note { get; set; }//ND Yêu cầu
        public Nullable<System.DateTime> FinishDateExpected { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public int EmployeesCreateID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public string Status { get; set; }
        public string StatusApprove { get; set; }
        public string StatusEdit { get; set; }
        public string Position { get; set; }
        public bool IsObsolete { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
    }
}
