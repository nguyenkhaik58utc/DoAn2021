using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;

namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    //[SystemAuthorize(CheckAuthorizeSuperAdmin = true)]
    [BaseSystem(Name = "Hỗ trợ hệ thống", IsActive = true, IsShow = true, StartAction = "UpdateSystem", Parent = GroupSystemController.Code)]
    public class SystemSupportController : BaseController
    {
        private SystemService systemService = new SystemService();
        
        [BaseSystem(Name = "Nâng cấp hệ thống", IsActive = true, IsShow = true)]
        public ActionResult UpdateSystem()
        {
            systemService.UpdateSystemAuto(User.ID);
            return View();
        }

        [BaseSystem(Name = "Phục hồi hệ thống", IsActive = true, IsShow = true )]
        public ActionResult RestoreSystem()
        {
            //systemService.RestoreSystemAuto(User.ID);
            return View();
        }
    }
}
