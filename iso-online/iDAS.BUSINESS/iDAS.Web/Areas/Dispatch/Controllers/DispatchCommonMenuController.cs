
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [BaseSystem(Name = "Danh mục chung ", IsActive = true, IsShow = true, Position = 1, IsGroup = true, Icon = "Folder")]
    public class DispatchCommonMenuController : BaseController
    {
        public const string Code = "DispatchCommonMenu";
    }
}
