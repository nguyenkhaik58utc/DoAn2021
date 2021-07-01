using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentProfileInterviewItem
    {
        public int ID { get; set; }
        public int ProfileID { get; set; }
        public int PlanID { get; set; }
        public string ProfileName { get; set; }
        public int RequirementID { get; set; }
        public bool IsEdit { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
    public class RecruitmentProfileIneterviewSelectItem
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Gender { get; set; }
        public int ProfileID { get; set; }
        public string RequirementName { get; set; }
        public int? RequirementID { get; set; }
        public bool IsEdit { get; set; }
        public bool IsSend { get { return !IsEdit; } }
        public bool IsSelect { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public string Status
        {
            get { return getStatus(); }
            set { }
        }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsEmployee { get; set; } 
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
    }
}