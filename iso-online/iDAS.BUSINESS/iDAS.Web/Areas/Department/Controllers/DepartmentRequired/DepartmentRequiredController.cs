using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Bối cảnh tổ chức", IsActive = true, IsShow = true, Position = 3, Icon = "HouseLink", IsGroup = true)]
    public class DepartmentRequiredController : BaseController
    {
        public const string Code = "DepartmentRequired";
    }
}
