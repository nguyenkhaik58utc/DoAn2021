using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Model
{
    public class V3_ProfilePermissionSaveDTO
    {
        public int ID { get; set; }
        public int DepartmentTitleFromID { get; set; }
        public int DepartmentTitleToID { get; set; }
        public bool IsView { get; set; }
        public bool IsUpdate { get; set; }
    }
}