using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerOrderItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int? ContractID { get; set; }
        public int ServiceID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public System.DateTime Time { get; set; }
        public bool IsPause { get; set; }
        public bool IsFinish { get; set; }
        public bool IsSuccess { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public string ActionForm { get; set; }
        public String SuccessStatus 
        {
            get
            {
                return (IsSuccess ? "<span style=\"color:blue\">Thành công</span>" :
                    (IsFinish ? "<span style=\"color:red\">Thất bại</span>" : "<span style=\"color:gray\">Đang thực hiện</span>"));
            }
            set{}
        }
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

        public AttachmentFileItem AttachmentFile { get; set; }

        public bool IsPrice { get; set; }
    }
    public class CustomerOrderSelected : CustomerOrderItem
    {
        public bool IsSelect { get; set; }
    }
    /// <summary>
    /// CuongPC
    /// </summary>
    public class ResultInsert
    {
        public int Order_ID { get; set; }
        public Nullable<Boolean> PrintActive { get; set; }
    }
}