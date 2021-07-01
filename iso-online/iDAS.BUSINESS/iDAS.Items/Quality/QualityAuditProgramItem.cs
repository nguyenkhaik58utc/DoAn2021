using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class QualityAuditProgramItem:ApprovalItem
    {
        private bool _IsNew = true;
        public DateTime EndTimePlan;   
        public int ID { get; set; }
        public HumanEmployeeViewItem Create { get; set; }
        public int? CreateByEmployeeID { get; set; }
        public int AuditPlanID { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
    
        public bool IsNew {
            get
            {
                return _IsNew;
            }
            set {
                _IsNew = value;
            } 
        }
        public string Content { get; set; }
        public string ListDepartment { get; set; }
        public string ListISOIndexAudit { get; set; }
        public string ListModuleAudit { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }      
         public string Status
        {
            get
            {
                return getStatus();
            }
            set
            {
            }
        }
         private string getStatus()
         {
             string result = "";
             EStatus status = EStatus.New;
             if (!IsEdit)
             {
                 status = EStatus.Wait;
             }
             if (IsEdit && IsApproval)
             {
                 status = EStatus.Edit;
             }
             if (IsApproval && IsAccept)
             {
                 status = EStatus.Perform;
             }
             if (IsApproval && !IsAccept && !IsEdit)
             {
                 status = EStatus.NotApproval;
             }
             switch (status)
             {
                 case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                 case EStatus.Wait: result = "<span style=\"color:black\">Chờ duyệt</span>"; break;
                 case EStatus.Edit: result = "<span style=\"color:#1efe00\">Sửa đổi</span>"; break;
                 case EStatus.NotApproval: result = "<span style=\"color:#41519f\">Không duyệt</span>"; break;
                 case EStatus.Perform: result = "<span style=\"color:#8e210b\">Thực hiện</span>"; break;
                 default: result = "<span style=\"color:#045fb8\">Mới</span>"; break;
             }
             return result;
         }
        private enum EStatus
        {
            New,
            Wait,
            Edit,
            NotApproval,
            Perform           
        }
        public DateTime DatePerform { get; set; }
        public string strDatePerform { get; set; }

        public string strTimePerform { get; set; }

        public string ListEmployee { get; set; }

        public DateTime StartTimePlan { get; set; }
    }
}
