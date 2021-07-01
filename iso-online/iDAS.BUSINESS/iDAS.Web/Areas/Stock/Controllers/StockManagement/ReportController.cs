
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iDAS.Core;
using System.Web.Mvc;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Báo cáo", Icon = "ReportPicture", IsActive = true, IsShow = true, Parent = GroupManageStockController.Code, IsGroup = true, Position = 5)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ReportController : BaseController
    {
        public const string Code = "Report";
        //
        // GET: /Stock/Report/


    }
}
