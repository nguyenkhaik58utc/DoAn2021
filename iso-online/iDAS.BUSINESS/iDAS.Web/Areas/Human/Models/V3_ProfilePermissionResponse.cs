using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Model
{
    public class V3_ProfilePermissionResponse
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string TitleName { get; set; }
        public int DepartmentTitleID { get; set; }
        public int Position { get; set; }
        public string StringPosition { get; set; }
        public int DepartmentTitleFromID { get; set; }
        public int DepartmentTitleToID { get; set; }
        public bool IsView { get; set; }
        public bool IsUpdate { get; set; }
    }
}