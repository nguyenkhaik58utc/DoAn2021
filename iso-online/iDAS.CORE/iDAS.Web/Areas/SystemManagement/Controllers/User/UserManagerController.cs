using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;

namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    [SystemAuthorize()]
    [BaseSystem(Name = "Quản trị người dùng", IsActive = true, IsShow = true)]
    public partial class UserManagerController : BaseController
    {
        [BaseSystem(Name = "Quản lý tổ chức", IsActive = true, IsShow = true)]
        public ActionResult GroupManager() {
            return View();
        }

        [BaseSystem(Name = "Quản lý thành viên", IsActive = true, IsShow = true)]
        public ActionResult UserManager()
        {
            return View();
        }

        [BaseSystem(Name = "Quản lý phân quyền", IsActive = true, IsShow = true)]
        public ActionResult PermissionManager()
        {
            return View();
        }
    }
}
