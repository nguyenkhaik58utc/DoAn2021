using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentCriteriaItem
    {
        public int ID { get; set; }
        public HumanRoleViewItem Role { get; set; }
        //public int RoleID { get; set; }
        ///// <summary>
        ///// Tên chức danh
        ///// </summary>
        //public string RoleName { get; set; }
        ///// <summary>
        ///// Tên đơn vị
        ///// </summary>
        //public string DepartmentName { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public int Factor { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string ActionForm { get; set; }
    }
   
}