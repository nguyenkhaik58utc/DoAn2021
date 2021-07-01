using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Risk.Controllers
{
    [BaseSystem(Name = "Đánh giá rủi ro", Icon = "FolderMagnify", IsActive = true, IsShow = true, Position = 2, IsGroup = true)]
    public class RiskAnalyticController : BaseController
    {
        public const string Code = "RiskAnalytic";
    }
}
