using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductDevelopmentRequirementPlanItem : QualityPlanItem
    {
        public int QualityPlanID { get; set; }
        public int ProductDevelopmentRequirementPlanID { get; set; }
        public int ProductDevelopmentRequirementID { get; set; }
        public Nullable<int> CreateByEmployeeID { get; set; }
        public Nullable<int> ProductionRequirementID { get; set; }
        public Nullable<int> UpdateByEmployeeID { get; set; }
        public decimal RateFinish { get; set; }

        public bool IsPause { get; set; }
        public bool IsCancel { get; set; }
        public bool IsFinish { get; set; }
        public string StatusProduction
        {
            get { return getStatusProduction(); }
            set { }
        }
        private string getStatusProduction()
        {
            string result = "";
            EStatus status = EStatus.NotRequirement;
            if (ProductionRequirementID!=0)
            {
                status = EStatus.Requirement;
            }
            if (IsPause)
            {
                status = EStatus.Pause;
            }
            if (IsCancel)
            {
                status = EStatus.Cancel;
            }
            if (IsFinish)
            {
                status = EStatus.Finish;
            }
            switch (status)
            {
                case EStatus.NotRequirement: result = "<span style=\"color:green\">Chưa có yêu cầu</span>"; break;
                case EStatus.Requirement: result = "<span style=\"color:red\">Đã yêu cầu</span>"; break;
                case EStatus.Pause: result = "<span style=\"color:#8e210b\">Tạm dừng</span>"; break;
                case EStatus.Cancel: result = "<span style=\"color:blue\">Hủy</span>"; break;
                case EStatus.Finish: result = "<span style=\"color:#41519f\">Hoàn thành</span>"; break;
                default: result = "<span style=\"color:#045fb8\">Mới</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            NotRequirement,
            Requirement,
            Pause,
            Cancel,
            Finish
        }
    }
}
