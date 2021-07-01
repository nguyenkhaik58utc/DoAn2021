using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class RiskControlItem  : ApprovalItem
    {
        public int ID { get; set; }
        public int RiskAuditID { get; set; }
        public int RiskID { get; set; }
        public string ListSolutionIds { get; set; }
        public float LikeLiHood { get; set; }
        public int RiskTreatmentID { get; set; }
        public Nullable<System.DateTime> CompleteTime { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public bool IsAcceptAudit { get; set; }
        public int QualityNCID { get; set; }
        public string ActionForm { get; set; }
        public float Impact { get; set; }
        public float NowConsequence { get; set; }
        public float RiskValue { get; set; }
        public int RiskCategoryID { get; set; }
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

        public int RiskLevelID { get; set; }

        public string Controls { get; set; }

        public float NowLikeLiHood { get; set; }

        public float NowImpact { get; set; }

        public string ListRiskTreatmentID { get; set; }

        public float Consequence { get; set; }

        public float NowRiskValue { get; set; }

        public int NowRiskLevelID { get; set; }
    }
}
