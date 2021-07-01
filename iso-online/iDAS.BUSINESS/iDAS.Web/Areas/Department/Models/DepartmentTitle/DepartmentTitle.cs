using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models
{
    public class DepartmentTitle
    {
        public int ID { get; set; }
        public string TitleName { get; set; }
        public string DepartmentName { get; set; }
        public string AliasName { get; set; }
        public int DepartmentID { get; set; }
        public int TitleID { get; set; }
        public string Responsible { get; set; }
        public string Power { get; set; }
        public string Note { get; set; }
        public int? ReportToID { get; set; }
        public string ReportToName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public int UserID { get; set; }

        public bool? IsRoleUpdate { get; set; }

        public HumanDepartmentViewItem Department { get; set; }

        public DepartmentTitle()
        {
            Department = new HumanDepartmentViewItem();
        }

    }
}