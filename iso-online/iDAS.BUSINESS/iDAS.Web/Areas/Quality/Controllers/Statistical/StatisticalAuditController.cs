using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Items;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Đánh giá", Icon = "ChartPie", IsActive = true, IsShow = false, Position = 3, Parent = GroupStatisticalController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class StatisticalAuditController : BaseController
    {
    }
}
