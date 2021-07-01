using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iDAS.Items
{
    public class TaskPersonalItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TaskID { get; set; }
        public int Parent { get; set; } 
        public int EmployeeID { get; set; }
        public bool IsNew { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreate { get; set; }
        public bool IsApproval { get; set; }
        public bool IsComplete { get; set; }
        public decimal Rate { get; set; }
        public bool IsPerform { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public TaskItem TaskInfo { get; set; }
        public TaskPersonalItem() {
            TaskInfo = new TaskItem();
        }
        public bool Leaf { get; set; }
    }
}
