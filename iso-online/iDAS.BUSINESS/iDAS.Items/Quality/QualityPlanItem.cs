using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Utilities;

namespace iDAS.Items
{
    public class QualityPlanItem : ApprovalItem
    {
        public int ID { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public Nullable<int> TargetID { get; set; }
        public String TargetName { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public bool Type { get; set; }
        [Display(Name = "Bắt đầu" + "<span style='color:red'> (*)</span>")]
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<decimal> Cost { get; set; }
       
        public bool IsDelete { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public bool IsEnd { get; set; }
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
            if (IsEnd)
            {
                status = EStatus.End;
            } 
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Change: result = "<span style=\"color:red\">Sửa đổi</span>"; break;
                case EStatus.ApprovalWait: result = "<span style=\"color:#8e210b\">Chờ duyệt</span>"; break;
                case EStatus.Success: result = "<span style=\"color:blue\">Duyệt</span>"; break;
                case EStatus.Fail: result = "<span style=\"color:#41519f\">Không duyệt</span>"; break;
                case EStatus.End: result = "<span style=\"gray\">Kết thúc</span>"; break;
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
            End
        }

        public bool Leaf { get; set; }

        public DateTime? EndAt { get; set; }

        public int CreateUserID { get; set; }
    }
}
