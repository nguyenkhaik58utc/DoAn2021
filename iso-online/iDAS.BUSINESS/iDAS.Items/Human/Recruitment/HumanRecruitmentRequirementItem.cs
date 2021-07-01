using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentRequirementItem : ApprovalItem
    {
        public int ID { get; set; }
        public HumanRoleViewItem Role { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int NumberIsEmployee { get; set; }
        public System.DateTime DateRequired { get; set; }
        public string Form { get; set; }
        public string Reason { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public String CreateByName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public String UpdateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
        public string Status {
            get { return getStatus(); }
            set { }
        }
        public bool IsComplete { get; set; }
        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.New;
            if (!IsEdit) {
                status = EStatus.ApprovalWait;
            }
            if (IsEdit && IsApproval){
                status = EStatus.Change;
            }
            if (IsApproval && IsAccept) {
                status = EStatus.Success;
            }
            if (IsApproval && !IsAccept && !IsEdit)
            {
                status = EStatus.Fail;
            }
            if (IsComplete) status = EStatus.Complete;
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Change: result = "<span style=\"color:red\">Sửa đổi</span>"; break;
                case EStatus.ApprovalWait: result = "<span style=\"color:#8e210b\">Chờ duyệt</span>"; break;
                case EStatus.Success: result = "<span style=\"color:blue\">Duyệt</span>"; break;
                case EStatus.Fail: result = "<span style=\"color:#41519f\">Không duyệt</span>"; break;
                case EStatus.Complete: result = "<span style=\"color:green\">Hoàn thành</span>"; break;
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
            Complete,
        }

        public int CreateUserID { get; set; }
    }
   
}