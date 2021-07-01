using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Tồn kho theo số lượng", Icon = "PageWhiteText", IsActive = true, IsShow = true, Parent = ReportController.Code, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ReportByQuantityController : BaseController
    {
        //
        // GET: /Stock/ReportByQuantity/
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
