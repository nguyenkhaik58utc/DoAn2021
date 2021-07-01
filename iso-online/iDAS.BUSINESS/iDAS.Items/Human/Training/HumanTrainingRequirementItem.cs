using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanTrainingRequirementItem : iDAS.Items.ApprovalItem
    {
        public int ID { get; set; }
        public HumanEmployeeViewItem RequireBy { get; set; }
        public string Content { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public String CreateByName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public String UpdateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
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

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string CreateUserName { get; set; }

        public int CreateUserID { get; set; }
    }

    public class HumanTrainingRequirementSelectItem : HumanTrainingRequirementItem
    {
        public bool IsSelected { get; set; }
    }
}