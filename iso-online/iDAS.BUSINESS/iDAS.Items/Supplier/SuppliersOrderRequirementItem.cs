using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SuppliersOrderRequirementItem : ApprovalItem
    {
        public SuppliersOrderRequirementItem()
        {
            this.SuppliersOrderRequirementDetails = new HashSet<SuppliersOrderRequirementDetailItem>();
        }
    
        public int ID { get; set; }
        public string CODE { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusDisp
        {
            get { return getStatus(); }
            set { }
        }
        public string StatusDispExcel
        {
            get { return getStatusExcel(); }
            set { }
        }
        private string getStatus()
        {
            string result = "";
            EStatus status = (EStatus)Enum.ToObject(typeof(EStatus), Status.HasValue ? Status.Value : 1);
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
        private string getStatusExcel()
        {
            string result = "";
            EStatus status = (EStatus)Enum.ToObject(typeof(EStatus), Status.HasValue ? Status.Value : 1);
            switch (status)
            {
                case EStatus.New: result = "Mới"; break;
                case EStatus.Change: result = "Sửa đổi"; break;
                case EStatus.ApprovalWait: result = "Chờ duyệt"; break;
                case EStatus.Success: result = "Duyệt"; break;
                case EStatus.Fail: result = "Không duyệt"; break;
                default: result = "Mới"; break;
            }
            return result;
        }
        private enum EStatus
        {
            New = 1,
            Change = 2,
            ApprovalWait = 3,
            Success = 4,
            Fail = 5
        }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }        
        public HumanEmployeeViewItem CreateUser { get; set; }
        public string CreateUserName
        {
            get;
            set;
        }
        public int CreateUserID
        {
            get;
            set;
        }
        public virtual ICollection<SuppliersOrderRequirementDetailItem> SuppliersOrderRequirementDetails { get; set; }
    }
}
