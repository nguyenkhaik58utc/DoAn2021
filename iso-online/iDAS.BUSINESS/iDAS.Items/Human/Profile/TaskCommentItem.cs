using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class TaskCommentItem
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int TaskID { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public TaskItem Task { get; set; }

        public string ActionForm { get; set; }

        public string EmployeeName { get; set; }

        public HumanEmployeeViewItem Employee { get; set; }
        public TaskCommentItem()
        {
            this.Employee = new HumanEmployeeViewItem();
        }

        public bool IsEdit { get; set; }
    }
}
