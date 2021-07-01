using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Báo cáo tồn kho tổng hợp", Icon = "PageWhiteText", IsActive = true, IsShow = true, Parent = ReportController.Code, Position = 5)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ReportTotalController : BaseController
    {
        //
        // GET: /Stock/ReportTotal/
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
