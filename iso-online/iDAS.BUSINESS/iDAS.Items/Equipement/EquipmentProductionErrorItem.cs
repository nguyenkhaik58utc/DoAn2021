using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class EquipmentProductionErrorItem
    {
        public int ID { get; set; }
        public int EquipmentProductionID { get; set; }
        public HumanDepartmentViewItem HumanDepartment { get; set; }
        public HumanEmployeeViewItem HumanEmployee { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public bool IsNew { get; set; }
        public bool IsFixed { get; set; }
        public string Content { get; set; }
        public string Cause { get; set; }
        public string Solution { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Problem { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
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
            EStatus status = EStatus.Fixing;
            if (IsFixed)
            {
                status = EStatus.Fixed;
            }
            switch (status)
            {
                case EStatus.Fixing: result = "<span style=\"color:red\">Đang sửa</span>"; break;
                case EStatus.Fixed: result = "<span style=\"color:#8e210b\">Đã sửa</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            Fixing,
            Fixed
        }
    }
}
