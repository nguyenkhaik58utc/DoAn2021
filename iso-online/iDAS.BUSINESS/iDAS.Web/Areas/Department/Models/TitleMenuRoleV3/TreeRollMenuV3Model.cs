using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models.TitleMenuRoleV3
{
    public class TreeRollMenuV3Model
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int RowLevel { get; set; }
        public bool IsPermission { get; set; }
        public string DisplayNameInforOnGrid { get; set; }
    }
}