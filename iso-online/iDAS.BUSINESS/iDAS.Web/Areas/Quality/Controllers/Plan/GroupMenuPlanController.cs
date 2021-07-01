using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Quản lý kế hoạch", Icon = "PageGear", IsActive = true, IsShow = true, Position = 2, IsGroup = true)]
    public class GroupMenuPlanController : BaseController
    {
        //
        // GET: /Quality/GroupMenuAudit/
        public const string Code = "GroupMenuPlan";

    }
}
