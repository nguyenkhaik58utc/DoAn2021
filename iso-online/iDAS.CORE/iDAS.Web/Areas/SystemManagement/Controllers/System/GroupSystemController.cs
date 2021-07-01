using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    [BaseSystem(Name = "nhom hệ thống", IsActive = true, IsShow = true, IsGroup=true )]
    public class GroupSystemController : BaseController
    {
        //
        // GET: /SystemManagement/GroupSystem/
        public const string Code = "GroupSystem";
        public ActionResult Index()
        {
            return View();
        }

    }
}
