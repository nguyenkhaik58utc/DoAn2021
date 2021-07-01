using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [BaseSystem(Name = "Thống kê", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 4, IsGroup = true)]
    public class GroupStatisticalDispatchMenuController : BaseController
    {
        //
        // GET: /Quality/GroupMenuTarget/
        public const string Code = "GroupStatisticalDispatchMenu";

    }
}
