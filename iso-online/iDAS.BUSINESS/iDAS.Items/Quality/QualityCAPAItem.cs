using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityCAPAItem : ApprovalItem
    {
        public int ID { get; set; }
        public int RequireID { get; set; }
        public bool IsComplete { get; set; }

        public bool IsPerformming { get; set; }
        public bool IsCaculatorComplete { get; set; }
        public bool IsOverTime { get; set; }

        public bool IsPass { get; set; }
        public Nullable<System.DateTime> CompleteTime { get; set; }
        public Nullable<System.DateTime> CompleteRealTime { get; set; }
        public string Cause { get; set; }
        public string Solution { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        //---Thông tin yêu cầu khắc phục phòng ngừa
        public string Code { get; set; }
        public string Name { get; set; }
        public HumanEmployeeViewItem CheckBy { get; set; }
        public Nullable<System.DateTime> CheckAt { get; set; }
        public string CheckNote { get; set; }
        //-----------------------------------------------------------------------------------------

        //--Trạng thái bản ghi---------------------------------------------------------------------
        public string Result
        {
            get
            {
                if (IsComplete)
                    return "<span style=\"color:blue\">Hoàn Thành</span>";
                return "<span style=\"color:gray\">Chưa hoàn thành</span>";
            }
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
            if (ID == 0)
            {
                status = EStatus.NewReceive;
            }
            if (IsPerformming)
            {
                status = EStatus.Performming;
            }
            if (IsCaculatorComplete)
            {
                status = EStatus.Complete;
            }
            if (IsOverTime)
            {
                status = EStatus.OverTime;
            }
            if (IsCaculatorComplete && IsOverTime)
            {
                status = EStatus.CompleteButOverTime;
            }
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Change: result = "<span style=\"color:red\">Sửa đổi</span>"; break;
                case EStatus.ApprovalWait: result = "<span style=\"color:#8e210b\">Chờ duyệt</span>"; break;
                case EStatus.Success: result = "<span style=\"color:blue\">Duyệt</span>"; break;
                case EStatus.Fail: result = "<span style=\"color:#41519f\">Không duyệt</span>"; break;
                case EStatus.NewReceive: result = "<span style=\"color:red\">Chưa có hành động</span>"; break;
                case EStatus.Performming: result = "<span>Đang thực hiện</span>"; break;
                case EStatus.Complete: result = "<span>Hoàn thành</span>"; break;
                case EStatus.CompleteButOverTime: result = "<span>Hoàn thành nhưng quá hạn</span>"; break;
                case EStatus.OverTime: result = "<span>Quá hạn</span>"; break;
                default: result = "<span style=\"color:#045fb8\">Mới</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            //-- chưa thực hiện hành động
            NewReceive,
            //--
            New,
            Change,
            ApprovalWait,
            Success,
            Fail,
            //Đang thực hiện
            Performming,
            // Hoàn thành
            Complete,
            // Hoàn thành nhưng quá hạn
            CompleteButOverTime,
            // Quá hạn
            OverTime,
        }
        //-----------------------------------------------------------------------------------------------
    }
}
