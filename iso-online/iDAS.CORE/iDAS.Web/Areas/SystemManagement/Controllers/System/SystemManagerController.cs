using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    //[SystemAuthorize(CheckAuthorizeSuperAdmin = true)]
    [BaseSystem(Name = "Quản trị hệ thống", IsActive = true, IsShow = true, StartAction = "ModuleManager", Parent = GroupSystemController.Code)]
    public partial class SystemManagerController : BaseController
    {
        private SystemService systemService = new SystemService();

        [BaseSystem(Name = "Quản lý Module", IsActive = true, IsShow = true)]
        public ActionResult ModuleManager()
        {
            return View();
        }

        [BaseSystem(Name = "Quản lý nhóm chức năng", IsActive = true, IsShow = true)]
        public ActionResult FunctionManager()
        {
            return View();
        }

        [BaseSystem(Name = "Quản lý chức năng", IsActive = true, IsShow = true)]
        public ActionResult ActionManager()
        {
            return View();
        }

        public ActionResult LoadIcons(int start, int limit)
        {
            int total;
            return this.Store(Ultility.GetIcons(start, limit, out total), total);
        }
    }
}
