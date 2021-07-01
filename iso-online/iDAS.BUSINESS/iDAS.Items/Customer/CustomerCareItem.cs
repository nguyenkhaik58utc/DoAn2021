using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerCareItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public bool IsPause { get; set; }
        public bool IsFinish { get; set; }
        public string Method { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
        public string StatusValue { get; set; }
        public string Status
        {
            get { return getStatus(); }
            set { }
        }

        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.New;
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
                case EStatus.Pause: result = "<span style=\"color:red\">Tạm dừng</span>"; break;
                case EStatus.Finish: result = "<span style=\"color:blue\">Hoàn thành</span>"; break;
                default: result = "<span style=\"color:green\">Mới</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            New,
            Pause,
            Finish
        }
    }

}