using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [BaseSystem(Name = "Công văn đến", IsActive = true, Icon = "PackageIn", IsShow = true, Position = 3, IsGroup = true)]
    public class DispatchToMenuController : BaseController
    {
        //
        // GET: /Dispatch/DispatchToGroupMenu/
        public const string Code = "DispatchToMenu";
    }
}
