using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentResultItem : ApprovalItem
    {
        public int ID { get; set; }
        // Tên hồ sơ
        public string ProfileName { get; set; }
        // Vị trí tuyển dụng
        public string RoleName { get; set; }
        // Kế hoạch tuyển dụng
        public String PlanName { get; set; }
        public int PlanID { get; set; }
        public int RequirementID { get; set; }
        public int ProfileInterviewID { get; set; }
        // Điểm nhỏ nhất tính toán được từ các tiêu chí lựa chọn
        public int? CriteriaMinPoint { get; set; }
        // Điểm lớn nhất tính toán được từ các tiêu chí lựa chọn
        public int? CriteriaMaxPoint { get; set; }
        public int? TotalPoint { get; set; }
        public System.DateTime? StartTime { get; set; }
        public decimal Salary { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public string Status
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
    }
    public class RecruitmentProfileResultItem
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsPass { get; set; }
        public DateTime? Birthday { get; set; }
        public bool? Gender { get; set; }
        public int ProfileID { get; set; }
        public string RequirementName { get; set; }
        public int? RequirementID { get; set; }

        public int EmployeeID { get; set; }

        public bool IsTrial { get; set; }
    }
   
}