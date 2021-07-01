using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Quản lý lịch làm việc", Icon = "Folder", IsActive = true, IsShow = true, Position = 1, IsGroup = true)]
    public class CalendarGroupController : BaseController
    {
        //
        // GET: /Quality/GroupMenuTarget/
        public const string Code = "CalendarGroup";

    }
}
