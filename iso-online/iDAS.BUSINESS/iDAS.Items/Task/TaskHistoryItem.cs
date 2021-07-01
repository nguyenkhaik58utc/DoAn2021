using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TaskHistoryItem : ApprovalItem
    {
        public TaskHistoryItem()
        {
            Audit = new HumanEmployeeViewItem();
            Perform = new HumanEmployeeViewItem();
            Check = new HumanEmployeeViewItem();          
       }
        public HumanDepartmentViewItem Department { get; set; }
        public HumanDepartmentViewItem DepartmentOld { get; set; }
        public HumanEmployeeViewItem Audit { get; set; }
        public HumanEmployeeViewItem Perform { get; set; }
        public HumanEmployeeViewItem PerformOld { get; set; }
        public HumanEmployeeViewItem Change { get; set; }
        public HumanEmployeeViewItem Check { get; set; }
        public HumanEmployeeViewItem Finish { get; set; }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int ParentOldID { get; set; }
        public int TaskID { get; set; }   
        public int DepartmentID { get; set; }
        public int DepartmentOldID { get; set; }   
        public int CategoryID { get; set; }
        public int CategoryOldID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string NameOld { get; set; }
        public string Content { get; set; }
        public string ContentOld { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime StartTimeOld { get; set; }
        public System.DateTime EndTime { get; set; }
        public System.DateTime EndTimeOld { get; set; }
        public Nullable<System.DateTime> CompleteTime { get; set; }
        public int LevelID { get; set; }
        public int LevelOldID { get; set; }
        public string LevelName { get; set; }
        public string Color { get; set; }
        public string ColorOld { get; set; }
        public bool IsPause { get; set; }
        public bool IsFinish { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPass { get; set; }
        public int PerformBy { get; set; }
        public string PerformEmployeesName { get; set; }
        public int CheckBy { get; set; }
        public string CheckName { get; set; }
        public decimal Cost { get; set; }
        public decimal CostOld { get; set; }
        public string Note { get; set; }
        public string Reason { get; set; }
        public bool IsChange { get; set; }
        public string ChangeNote { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public AttachmentFileItem AttachmentFileOlds { get; set; }
        public string Status {
            get
            {
                return getStatus();
            }
            set
            {
            } 
        }

        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.NotApprove;
            if (IsChange)
            {
                status = EStatus.Approve;
            }            
            switch (status)
            {
                case EStatus.Approve: result = "<span style=\"color:green\">Đã điều chỉnh</span>"; break;                
                default: result = "<span style=\"color:red\">Yêu cầu điều chỉnh</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            NotApprove,
            Approve
        }
    }

}