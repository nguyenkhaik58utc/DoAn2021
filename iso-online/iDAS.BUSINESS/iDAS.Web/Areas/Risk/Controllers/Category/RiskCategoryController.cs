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
    [BaseSystem(Name = "Danh mục", Icon = "TagBlue", IsActive = true, IsShow = true, Position = 1, IsGroup = true)]
    public class RiskCategoryController : BaseController
    {
        public const string Code = "RiskCategory";
    }
}
