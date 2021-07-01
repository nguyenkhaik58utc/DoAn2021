using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Core.Controllers
{
    public class SystemController : BaseController
    {
        SystemSV systemSV = new SystemSV();
        public ActionResult UpdateToCenter()
        {
            bool success = true;
            try
            {
                systemSV.UpdateSystem(true);
            }
            catch {
                success = false;
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }
    }
}
