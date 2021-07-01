using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Xuất kho theo vật tư hàng hóa", Icon = "PageWhiteText", IsActive = true, IsShow = true, Parent = ReportController.Code, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ReportOutwardByProductController : BaseController
    {
        //
        // GET: /Stock/ReportOutwardByProduct/
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
