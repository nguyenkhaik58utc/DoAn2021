using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Service.Controllers
{

    [BaseSystem(Name = "Thống kê", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 7, IsGroup = true)]
    public class GroupStatisticalServiceController : BaseController
    {
        //
        // GET: /Quality/GroupMenuTarget/
        public const string Code = "GroupStatisticalService";

    }
}
