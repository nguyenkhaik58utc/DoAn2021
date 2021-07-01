using iDAS.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Problem.Controllers
{
    public class MapController : Controller
    {
        private ProblemEventAPI api = new ProblemEventAPI();
        // GET: Problem/Map
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList()
        {
            var resp = api.GetList();
            int total = 0;
            if (resp.TotalRows != null)
                total = resp.TotalRows.Value;
            return Json(resp.Data, JsonRequestBehavior.AllowGet);
        }

    }
}