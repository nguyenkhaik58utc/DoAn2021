using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Items
{
    public class QualityCAPARequireItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public int NCID { get; set; }
      //  public EmployeeViewItem Employee { get; set; }
        public string Content { get; set; }
        public System.DateTime CompleteTime { get; set; }
        public Nullable<System.DateTime> CompleteRealTime { get; set; }
        public bool IsCompleteAuto { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<int> ParentID { get; set; }
        public HumanEmployeeViewItem Represent { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
