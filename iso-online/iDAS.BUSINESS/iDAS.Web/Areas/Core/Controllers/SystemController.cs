using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;

namespace iDAS.Web.Areas.Core.Controllers
{
    public class SystemController : BaseController
    {
        public ActionResult Setup(int customerID)
        {
            bool success = true;
            try
            {
                BusinessSystemSV systemSV = new BusinessSystemSV();
                success = systemSV.SetupSystem(customerID);
            }
            catch
            {
                success = false;
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(int customerID)
        {
            bool success = true;
            try
            {
                BusinessSystemSV systemSV = new BusinessSystemSV();
                success = systemSV.UpdateSystem(customerID);
            }
            catch
            {
                success = false;
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateToCenter()
        {
            bool success = true;
            try
            {
                CenterSystemSV systemSV = new CenterSystemSV();
                systemSV.UpdateSystem();
            }
            catch
            {
                success = false;
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }
    }
}
