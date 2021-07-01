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

namespace iDAS.Web.Areas.ISO.Controllers
{
    [BaseSystem(Name = "Quản lý rủi ro", Icon = "AsteriskYellow", IsActive = true, IsShow = true, IsGroup = true, Position = 6)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskGroupController : BaseController
    {
        public const string Code = "RiskGroup";
    }
}
