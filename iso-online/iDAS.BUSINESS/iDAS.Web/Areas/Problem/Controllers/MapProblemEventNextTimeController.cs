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
using iDAS.Items;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Problem.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thiết Bị Sản Xuất", IsActive = true, IsShow = true, Position = 2, Icon = "Table")]
    public class MapProblemEventNextTimeController : BaseController
    {
        public EquipmentProductionSV EquipmentProductionSV = new EquipmentProductionSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        // GET: Problem/MapProblemEventNextTime
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList()
        {
            var resp = EquipmentProductionSV.GetAll();
            return Json(resp, JsonRequestBehavior.AllowGet);
        }
    }
}