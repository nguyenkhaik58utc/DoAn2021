using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Thống kê", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 8, IsGroup = true)]
    public class GroupStatisticalController : BaseController
    {
        public const string Code = "GroupStatistical";
    }
}
