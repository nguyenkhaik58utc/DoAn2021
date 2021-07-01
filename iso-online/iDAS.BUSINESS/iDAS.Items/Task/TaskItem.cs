using iDAS.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Utilities;
namespace iDAS.Items
{
    public class TaskItem
    {
        public string TotalTime { get; set; }
        private bool _IsNew = true;     
        private bool _IsComplete = false;
        private bool _IsAudit = false;
        private bool _IsPass = false;
        private bool _IsPause = false;
        public bool IsPrivate {get;set;}
        public string RoleCreateName { get; set; }
        public string AvatarCreateName { get; set; }
        public string DepartmentCreateName { get; set; }
        public string CreateByName { get; set; }
        public int EmployeesIDCreate { get; set; }
        public bool IsRequireCheck { get; set; }
        public bool IsResquestNew { get; set; }  
        public string Reason { get; set; }
        public TaskItem()
        {
            Create = new HumanEmployeeViewItem();
            Perform = new HumanEmployeeViewItem();
            Update = new HumanEmployeeViewItem();
        }
        public bool IsEdit{get;set;}
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Rate { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public bool IsNew
        {
            get;
            set;
        }     
        public bool IsComplete
        {
            get
            {
                return _IsComplete;
            }
            set
            {
                if (value)
                {
                    _IsNew = false;
                }
                _IsComplete = value;
            }
        }
        public bool IsAudit
        {
            get
            {
                return _IsAudit;
            }
            set
            {
                if (value)
                {
                    _IsNew = false;
                }
                _IsAudit = value;
            }
        }
        public bool IsPass
        {
            get
            {
                return _IsPass;
            }
            set
            {
                _IsPass = value;
            }
        }
        public bool IsPause
        {
            get
            {
                return _IsPause;
            }
            set
            {
                if (value)
                {
                    _IsNew = false;
                }
                _IsPause = value;
            }
        }
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public string LevelColor { get; set; }

        public System.DateTime StartTime { get; set; }     
        public System.DateTime EndTime { get; set; }
        public System.DateTime CompleteTime { get; set; }   
        public int PerformBy { get; set; }
        public HumanEmployeeViewItem Perform { get; set; }
        public HumanEmployeeViewItem Create { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public HumanEmployeeViewItem Update { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public bool IsOverTime
        {
            get
            {
                return EndTime < DateTime.Now ? true : false;
            }
        }
        public string Status
        {
            get
            {
                return getStatus();
            }
        }

        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.New;
            if (!IsEdit)
            {
                status = EStatus.Perform;
            }
            if (!IsNew && !IsComplete && Rate != 100 && EndTime <= DateTime.Now) { status = EStatus.OverTime; }
            if (IsComplete && Rate == 100 && EndTime >= CompleteTime) { status = EStatus.Complete; }
            if (IsComplete && Rate == 100 && EndTime < CompleteTime) { status = EStatus.CompleteOverTime; }
            if (IsPause) { status = EStatus.Pause; }          
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:#045fb8\">Mới</span>"; break;
                case EStatus.Perform: result = "<span style=\"color:#293955\">Thực hiện</span>"; break;
                case EStatus.Complete: result = "<span style=\"color:blue\">Hoàn thành</span>"; break;
                case EStatus.Pause: result = "<span style=\"color:red\">Tạm dừng</span>"; break;
                case EStatus.OverTime: result = "<span style=\"color:#8e210b\">Quá hạn</span>"; break;
                case EStatus.CompleteOverTime: result = "<span style=\"color:#3300f1\">Hoàn thành quá hạn</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            New,
            Perform,
            Complete,
            Pause,
            OverTime,
            CompleteOverTime
        }

        public int IDRef { get; set; }
        public int ParentID { get; set; }
        public bool Leaf { get; set; } 
        public AttachmentFileItem AttachmentFiles { get; set; }
    }
}
