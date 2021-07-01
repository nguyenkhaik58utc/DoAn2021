using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iDAS.Items
{
    public class CustomerContractItem : ApprovalItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> StatTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public bool IsPause { get; set; }
        public bool IsFinish { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsSend { get; set; }
        public string Content { get; set; }

        public string Note { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> TotalOrder { get; set; }
        public string DeptType { get; set; }
        public decimal? AccountingPaymentTotal { get; set; }
        public DateTime? MaxTime { get; set; }
        public System.DateTime FinishDate { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public HumanEmployeeViewItem Employee { get; set; }
        #region Gửi cho kế toán
        /// <summary>
        /// Khách hàng đã ký value = true
        /// </summary>
        public bool IsSignCustomer { get; set; }
        public Nullable<System.DateTime> SignCustomerAt { get; set; }
        /// <summary>
        /// trạng thái đã xem hợp đồng của lãnh đạo
        /// </summary>
        public bool IsSignReview { get; set; }
        /// <summary>
        /// lãnh đạo đã ký value=true
        /// </summary>
        public bool IsSignCompany { get; set; }
        public Nullable<System.DateTime> SignCompanyAt { get; set; }
        public bool IsStart { get; set; }
        public bool IsCancel { get; set; }
        public string CancelNote { get; set; }
        public Nullable<DateTime> StartRealTime { get; set; }
        #endregion

        public Nullable<System.DateTime> CreateAt { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsSelected { get; set; }
        public string ActionForm { get; set; }
        public decimal RateFinish { get; set; }
        public string Status
        {
            get { return getStatus(); }
            set { }
        }
        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.New;
            if (IsSend)
                status = EStatus.AccountingSended;
            if (IsSend && !IsEdit && !IsApproval)
            {
                status = EStatus.ApprovalWait;
            }
            if (IsSend && IsEdit && IsApproval)
            {
                status = EStatus.ChangePayment;
            }
            if (IsSend && IsApproval && IsAccept)
            {
                status = EStatus.SuccessPayment;
            }
            if (IsSend && IsApproval && !IsAccept && !IsEdit)
            {
                status = EStatus.FailPayment;
            }
            if (IsSend && IsApproval && IsAccept && !IsEdit && IsSignCustomer)
            {
                status = EStatus.SignWait;
            }
            if (IsSend && IsApproval && IsAccept && !IsEdit && IsSignCustomer && IsSignReview && IsSignCompany)
            {
                status = EStatus.SignAccept;
            }
            if (IsStart)
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
            if (!IsFinish && DateTime.Now > EndTime)
            {
                status = EStatus.OverTime;
            }
            if (IsFinish && CompleteDate > EndTime)
            {
                status = EStatus.OverTime;
            }
            if (IsCancel)
            {
                status = EStatus.Cancel;
            }
            if (IsSuccess)
            {
                status = EStatus.Sucess;
            }
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.AccountingSended: result = "<span>Gửi kế toán</span>"; break;
                case EStatus.ApprovalWait: result = "<span>Chờ duyệt</span>"; break;
                case EStatus.ChangePayment: result = "<span>Sửa đổi</span>"; break;
                case EStatus.SuccessPayment: result = "<span>Duyệt</span>"; break;
                case EStatus.FailPayment: result = "<span>Không duyệt</span>"; break;
                case EStatus.SignWait: result = "<span>Chờ ký</span>"; break;
                case EStatus.SignAccept: result = "<span>Đã Ký</span>"; break;
                case EStatus.Doing: result = "<span>Thực hiện</span>"; break;
                case EStatus.Pause: result = "<span style=\"color:red\">Tạm dừng</span>"; break;
                case EStatus.Finish: result = "<span style=\"color:blue\">Hoàn thành</span>"; break;
                case EStatus.OverTime: result = "<span>Quá hạn</span>"; break;
                case EStatus.SuccessButOverTime: result = "<span style=\"color:violet\">Hoàn thành(QH)</span>"; break;
                case EStatus.Cancel: result = "<span style=\"color:red\">Hủy hợp đồng</span>"; break;
                case EStatus.Sucess: result = "<span>Kết thúc</span>"; break;
                default: result = "<span style=\"color:green\">Mới</span>"; break;
            }
            return result;

        }
        private enum EStatus
        {
            New,
            AccountingSended,
            ApprovalWait,
            ChangePayment,
            SuccessPayment,
            FailPayment,
            SignWait,
            SignAccept,
            Doing,
            Pause,
            Finish,
            OverTime,
            SuccessButOverTime,
            Cancel,
            Sucess
        }
        public AttachmentSelectItem Attachment { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
    }
}