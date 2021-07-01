using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionCommandItem
    {
        public int ID { get; set; }
        public Nullable<int> ProductionPlanID { get; set; }
        public Nullable<int> ProductionRequirementID { get; set; }
        public string Code { get; set; }
        public string BatchCode { get; set; }
        public int ProductionStageID { get; set; }
        public string ProductionName { get; set; }
        public HumanDepartmentViewItem HumanDepartment { get; set; }
        public int Quantity { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public System.DateTime? FinishTime { get; set; }
        public bool IsStart { get; set; }
        public bool IsPause { get; set; }
        public bool IsFinish { get; set; }
        public bool IsDelete { get; set; }
        public string Note { get; set; }
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
            if (IsStart)
            {
                status = EStatus.Doing;
            }
            if(IsPause)
            {
                status = EStatus.Pause;
            }
            if (IsFinish)
            {
                status = EStatus.Finish;
            }
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Chưa thực hiện</span>"; break;
                case EStatus.Doing: result = "<span>Đang thực hiện</span>"; break;
                case EStatus.Pause: result = "<span style=\"color:red\">Tạm dừng</span>"; break;
                case EStatus.Finish: result = "<span style=\"color:#993300\">Kết thúc</span>"; break;
                default: result = "<span style=\"color:green\">Chưa thực hiện</span>"; break;
            }
            return result;

        }
        private enum EStatus
        {
            New,
            Doing,
            Pause,
            Finish,
        }
    }
}
