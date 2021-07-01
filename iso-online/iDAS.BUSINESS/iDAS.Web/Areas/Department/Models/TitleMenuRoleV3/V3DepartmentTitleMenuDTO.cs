using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models.TitleMenuRoleV3
{
    public class V3DepartmentTitleMenuDTO
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int DepartmentTitleID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}