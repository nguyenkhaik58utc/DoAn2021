using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityMeetingItem : ApprovalItem
    {
        private bool _IsNew = true;
        private bool _IsFinish = false;
        public int ID { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public decimal Cost { get; set; }
      
        public bool IsNew {
            get
            {
                return _IsNew;
            }
            set
            {
                _IsNew = value;
            }
        }
       // public bool? IsResult { get; set; }
        public bool IsFinish {
            get
            {
                return _IsFinish;
            }
            set
            {
                if(value)
                {
                    IsNew = false;
                    IsEdit = false;
                    IsApproval = true;
                }
                _IsFinish = value;
            }
        }
        public string Address { get; set; }
        public string TaskPrepare { get; set; }
        public string Content { get; set; }
        //public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public AttachmentFileItem AttachmentFile { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
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
            if (IsFinish)
            {
                status = EStatus.Finish;
            }
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Change: result = "<span style=\"color:red\">Sửa đổi</span>"; break;
                case EStatus.ApprovalWait: result = "<span style=\"color:#8e210b\">Chờ duyệt</span>"; break;
                case EStatus.Success: result = "<span style=\"color:blue\">Duyệt</span>"; break;
                case EStatus.Fail: result = "<span style=\"color:#41519f\">Không duyệt</span>"; break;
                case EStatus.Finish: result = "<span style=\"color:#99bce8\">Kết thúc</span>"; break;
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
            Finish,
        }
    }
}
