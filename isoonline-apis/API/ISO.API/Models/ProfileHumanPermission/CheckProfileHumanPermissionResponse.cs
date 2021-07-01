using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.ProfileHumanPermission
{
    public class CheckProfileHumanPermissionResponse
    {
        public int DepartmentTitleToID { get; set; }
        public bool IsView { get; set; }
        public bool IsUpdate { get; set; }
    }
}
