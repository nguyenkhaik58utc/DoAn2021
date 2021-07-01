using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerCareResultItem
    {
        public int? ID { get; set; }
        public int? CareID { get; set; }
        public string Method { get; set; }
        public int CustomerID { get; set; }
        public int GroupCustomerID { get; set; }
        public bool IsSuccess { get; set; }
        public String SuccessStatus
        {
            get { return (IsSuccess ? "<span style=\"color:blue\">Thành công</span>" : "<span style=\"color:red\">Thất bại</span>"); }
            set { }
        }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Type { get; set; }
        public string Address { get; set; }
        public string ActionForm { get; set; }
        public bool IsCare { get; set; }
        public string CareName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsPause { get; set; }
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