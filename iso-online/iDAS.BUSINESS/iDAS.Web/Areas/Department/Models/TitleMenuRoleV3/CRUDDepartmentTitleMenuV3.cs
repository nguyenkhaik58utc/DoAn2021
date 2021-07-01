using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models.TitleMenuRoleV3
{
    public class CRUDDepartmentTitleMenuV3
    {
        public int DepartmentID { get; set; }
        public int TitleID { get; set; }
        public string lstAdd { get; set; }
        public string lstDelete { get; set; }
    }
}