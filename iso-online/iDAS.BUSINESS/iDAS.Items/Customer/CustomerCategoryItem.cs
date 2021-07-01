using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerCategoryItem
    {
        public int ID { get; set; }
        public HumanEmployeeViewItem Employee { get; set; }
        public string Name { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsParent { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ParentID { get; set; }
        public HumanDepartmentViewItem Departments { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
        public bool Leaf { get; set; }
    }
}