using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionPlanItem
    {
        public int ID { get; set; }
        public ProductionRequirementViewItem ProductionRequirement { get; set; }
        public ProductViewItem Product { get; set; }
        public float Quantity { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string PlanName
        {
            get
            {
                return "Kế hoạch " + StartTime.ToString("dd/MM/yyyy") + " - " + EndTime.ToString("dd/MM/yyyy");
            }
        }
        public bool IsDelete { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public bool IsDoing { get; set; }
        public bool IsFinish { get; set; }
        public string Status
        {
            get { return getStatus(); }
            set { }
        }
        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.New;
            if(IsDoing)
            {
                status = EStatus.Doing;
            }
            if (IsFinish)
            {
                status = EStatus.Finish;
            }
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Chưa thực hiện</span>"; break;
                case EStatus.Doing: result = "<span>Đang thực hiện</span>"; break;
                case EStatus.Finish: result = "<span style=\"color:#993300\">Kết thúc</span>"; break;
                default: result = "<span style=\"color:green\">Chưa thực hiện</span>"; break;
            }
            return result;

        }
        private enum EStatus
        {
            New,
            Doing,
            Finish,
        }
    }
}
