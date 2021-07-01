using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerCampaignItem : iDAS.Items.ApprovalItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public decimal Cost { get; set; }
        //public bool IsEdit { get; set; }
        //public bool IsApproval { get; set; }
        //public bool IsAccept { get; set; }
        //public bool? IsResult { get; set; }
        public bool IsPause { get; set; }
        public bool IsFinish { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<decimal> Income { get; set; }
        public string Note { get; set; }
        //public Nullable<int> ApprovalBy { get; set; }
        //public string ApprovalAvatar { get; set; }
        //public string ApproveNameBy { get; set; }
        //public Nullable<System.DateTime> ApprovalAt { get; set; }
        //public string ApprovalNote { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public string StatusValue { get; set; }
        public string ActiveStatus
        {
            get { return getActiveStatus(); }
            set { }
        }

        private string getActiveStatus()
        {
            string result = "";
            EActiveStatus status = EActiveStatus.New;
            if (IsPause)
            {
                status = EActiveStatus.Pause;
            }
            if (IsFinish)
            {
                status = EActiveStatus.Finish;
            }
            switch (status)
            {
                case EActiveStatus.New: result = "<span style=\"color:green\">Đang tiến hành</span>"; break;
                case EActiveStatus.Pause: result = "<span style=\"color:red\">Tạm dừng</span>"; break;
                case EActiveStatus.Finish: result = "<span style=\"color:blue\">Hoàn thành</span>"; break;
                default: result = "<span style=\"color:green\">Mới</span>"; break;
            }
            return result;
        }
        private enum EActiveStatus
        {
            New,
            Pause,
            Finish
        }
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
    }

}