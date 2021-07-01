using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionRequirementItem
    {
        private bool _IsNew = true;
        public int ID { get; set; }
        public HumanEmployeeViewItem HumanEmployee { get; set; }
        public ProductionLevelItem ProductionLevel { get; set; }
        public ProductViewItem Product { get; set; }
        public int Quantity { get; set; }
        public System.DateTime? FinishTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name{ get; set; }
        public bool IsNew
        {
            get
            {
                return _IsNew;
            }
            set
            {
                _IsNew = value;
            }
        }
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
            if (!_IsNew)
            {
                status = EStatus.Doing;
            }
            if (IsPause)
            {
                status = EStatus.Pause;
            }
            if (IsFinish)
            {
                status = EStatus.Finish;
            }
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Doing: result = "<span>Thực hiện</span>"; break;
                case EStatus.Pause: result = "<span style=\"color:red\">Tạm dừng</span>"; break;
                case EStatus.Finish: result = "<span style=\"color:#993300\">Kết thúc</span>"; break;
                default: result = "<span style=\"color:green\">Mới</span>"; break;
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
