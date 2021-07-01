using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityTargetItem : ApprovalItem
    {
        public QualityTargetItem()
        {
            this.CustomerCampaignTargets = new HashSet<CustomerCampaignTargetItem>();
            this.QualityPlans = new HashSet<QualityPlanItem>();
        }
    
        public int ID { get; set; }
        public Nullable<int> QualityTargetCategoryID { get; set; }
        public string QualityTargetCategoryName { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> QualityTargetLevelID { get; set; }
        public string CateName { get; set; }
        public string LevelName { get; set; }
        public string LevelColor { get; set; }
        public string ParentName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public System.DateTime CompleteAt { get; set; }
        public bool Type { get; set; }
        public string TypeName { get; set; }
        public bool IsEnd { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public string ApprovalName { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public Nullable<System.DateTime> MaxValueCompleteAt { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public string TargetName { get; set; }
        public Nullable<int> CreateByEmployeeID { get; set; }
        public Nullable<int> UpdateByEmployeeID { get; set; }
        public virtual ICollection<CustomerCampaignTargetItem> CustomerCampaignTargets { get; set; }
        public virtual ICollection<QualityPlanItem> QualityPlans { get; set; }
        public virtual QualityTargetCategoryItem QualityTargetCategory { get; set; }
        public HumanDepartmentViewItem Department { get; set; }

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

            if (!IsEdit && IsApproval && !IsAccept)
            {
                status = EStatus.NotApproval;
            }
            if (IsAccept)
            {
                status = EStatus.Perform;
            }
            if (IsAccept && !IsEnd && CompleteAt < DateTime.Now.AddDays(-1))
            {
                status = EStatus.OverTime;
            }            
            if (IsApproval && !IsAccept && IsEdit)
            {
                status = EStatus.Edit;
            }
            if (IsEnd)
            {
                status = EStatus.End;
            } 
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Wait: result = "<span style=\"color:black\">Chờ duyệt</span>"; break;
                case EStatus.Edit: result = "<span style=\"color:blue\">Sửa đổi</span>"; break;
                case EStatus.NotApproval: result = "<span style=\"color:#41519f\">Không duyệt</span>"; break; 
                case EStatus.Perform: result = "<span style=\"color:#8e210b\">Thực hiện</span>"; break;
                case EStatus.OverTime: result = "<span style=\"color:#8e210b\">Quá hạn</span>"; break;
                //case EStatus.Success: result = "<span style=\"color:#293955\">Thành công</span>"; break;
                //case EStatus.Fail: result = "<span style=\"color:#293955\">Thất bại</span>"; break;
                case EStatus.End: result = "<span style=\"gray\">Kết thúc</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            New,
            Wait,
            Edit,
            NotApproval,
            Perform,
            OverTime,
            //Success,
            //Fail,
            End
        }
        public DateTime? EndAt { get; set; }
        public bool Leaf;
        public string ActionForm { get; set; }
    }
}
