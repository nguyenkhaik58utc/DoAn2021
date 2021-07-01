using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class CopyMenuTitleRoleV3DTO
    {
        public int srcDepartmentID { get; set; }
        public int srcTitleID { get; set; }
        public int desDepartmentID { get; set; }
        public int desTitleID { get; set; }
    }
}
