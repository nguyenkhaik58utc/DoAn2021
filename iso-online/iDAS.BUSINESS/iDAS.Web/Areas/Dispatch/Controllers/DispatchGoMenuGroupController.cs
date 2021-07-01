using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [BaseSystem(Name = "Công văn đi ", IsActive = true, IsShow = true, Position = 2,Icon="PackageGo", IsGroup = true)]
    public class DispatchGoMenuGroupController : BaseController
    {
        public const string Code = "DispatchGoMenuGroup";
    }
}
