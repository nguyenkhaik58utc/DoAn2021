using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Quản lý mục tiêu", Icon = "ChartCurve", IsActive = true, IsShow = true, Position = 1, IsGroup = true)]
    public class GroupMenuTargetController : BaseController
    {
        //
        // GET: /Quality/GroupMenuTarget/
        public const string Code = "GroupMenuTarget";

    }
}
