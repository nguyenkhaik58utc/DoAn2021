using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Model
{
    public class V3_CheckProfileHumanPermissionResponse
    {
        public int DepartmentTitleToID { get; set; }
        public bool IsView { get; set; }
        public bool IsUpdate { get; set; }
    }
}