using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models.TitleMenuRoleV3
{
    public class CopyMenuTitleRoleV3
    {
        public int srcDepartmentID { get; set; }
        public int srcTitleID { get; set; }
        public int desDepartmentID { get; set; }
        public int desTitleID { get; set; }
    }
}