using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanAbsentItem
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public int HumantAbsentTypeID { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<int> DayNumber { get; set; }
        public string Contents { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> IsApproval { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public string ApprovalNote { get; set; }
        public Nullable<bool> IsEdit { get; set; }
        public Nullable<bool> IsAccept { get; set; }
        public Nullable<bool> HaftDay { get; set; }

        public virtual HumanAbsentTypeItem HumanAbsentType { get; set; }
    }
}
