using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DocumentCategoryItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public HumanDepartmentViewItem Department{ get; set; }
        public bool IsActive { get; set; }//ND Yêu cầu
        public string Note { get; set; }//ND Yêu cầu
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public bool IsDelete { get; set; }
    }
}
