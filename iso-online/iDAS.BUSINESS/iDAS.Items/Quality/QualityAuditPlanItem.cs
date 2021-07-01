using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityAuditPlanItem : ApprovalItem
    {
        private bool _IsNew = true;
        public int ID { get; set; }
        public int ISOID { get; set; }
        public int? CreateByEmployeeID { get; set; }
        public string ISOName { get; set; }    
        public string Code { get; set; }
        public HumanEmployeeViewItem Create { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }       
        public System.DateTime EndTime { get; set; }
        public bool IsNew
        {
            get
            {
                return _IsNew;
            }
            set
            {
                _IsNew = value;
            }
        }
        public string Scope { get; set; }
        public string Require { get; set; }
        public string Note { get; set; }
        public bool IsPass { get; set; }
        public string AuditNote { get; set; }
        public int NumberISOIndexPass { get; set; }
        public int NumberISOIndexNotPass { get; set; }
        public int NumberISOIndexNotAudit { get; set; }
        public int TotalISOIndexAudit { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateByName { get; set; }
        public int NumberM { get; set; }
        public int NumberObs { get; set; }
        public int NumberMedium { get; set; }
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
    }
}
