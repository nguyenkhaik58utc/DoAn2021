using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanProfileWorkTrialItem : ApprovalItem
    {
        public HumanProfileWorkTrialItem()
        {
            this.HumanProfileWorkTrialResults = new HashSet<HumanProfileWorkTrialResultItem>();
            this.Role = new HumanRoleViewItem();
            this.Manager = new HumanEmployeeViewItem();
            this.Approval = new HumanEmployeeViewItem();
            this.Employee = new HumanEmployeeViewItem();
        }
    
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public Nullable<int> HumanRoleID { get; set; }
        public HumanRoleViewItem Role { get; set; }
        public HumanEmployeeViewItem Manager { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Note { get; set; }        
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> DirectorApproval { get; set; }
        public Nullable<System.DateTime> DirectorApprovalAt { get; set; }
        public Nullable<int> ContractType { get; set; }
        public Nullable<System.DateTime> ContractStartTime { get; set; }
        public Nullable<int> ManagerID { get; set; }
        public virtual HumanEmployeeItem HumanEmployee { get; set; }
        public virtual ICollection<HumanProfileWorkTrialResultItem> HumanProfileWorkTrialResults { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string StatusAudit { get; set; }
        public int ProfileID { get; set; }
        public string StatusApproval
        {
            get { return getStatus(); }
            set { }
        }

        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.New;
            if (!IsEdit)
            {
                status = EStatus.ApprovalWait;
            }
            if (IsEdit && IsApproval)
            {
                status = EStatus.Change;
            }
            if (IsApproval && IsAccept)
            {
                status = EStatus.Success;
            }
            if (IsApproval && !IsAccept && !IsEdit)
            {
                status = EStatus.Fail;
            }
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Change: result = "<span style=\"color:red\">Sửa đổi</span>"; break;
                case EStatus.ApprovalWait: result = "<span style=\"color:#8e210b\">Chờ duyệt</span>"; break;
                case EStatus.Success: result = "<span style=\"color:blue\">Duyệt</span>"; break;
                case EStatus.Fail: result = "<span style=\"color:#41519f\">Không duyệt</span>"; break;
                default: result = "<span style=\"color:#045fb8\">Mới</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            New,
            Change,
            ApprovalWait,
            Success,
            Fail,
        }
        public string CreateByName { get; set; }

        public int CreateUserID { get; set; }

        public HumanEmployeeViewItem Employee { get; set; }

        public string ActionForm { get; set; }
    }
}
