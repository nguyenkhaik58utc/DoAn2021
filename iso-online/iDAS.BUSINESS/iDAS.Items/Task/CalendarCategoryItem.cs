using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public class CalendarCategoryItem
    {
        public int ID { get; set; }
        public Nullable<int> HumanDepartmentID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool Leaf { get; set; }
    }
}
