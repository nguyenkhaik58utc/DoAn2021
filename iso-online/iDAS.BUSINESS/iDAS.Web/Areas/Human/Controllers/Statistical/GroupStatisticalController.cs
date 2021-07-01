using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Thống kê", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 7, IsGroup = true)]
    public class GroupStatisticalController : BaseController
    {
        public const string Code = "GroupStatistical";
    }
}
