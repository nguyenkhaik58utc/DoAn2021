using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Thông tin tổ chức", IsActive = false, IsShow = false, Position = 3, Icon = "HouseLink", IsGroup = true)]
    public class DepartmentInfoController : BaseController
    {
        public const string Code = "DepartmentInfo";
    }
}
