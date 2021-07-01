using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
   [BaseSystem(Name = "Quản lý họp xem xét lãnh đạo", Icon = "GroupGear", IsActive = true, IsShow = true, Position = 5, IsGroup = true)]
    public class MeetingController : BaseController
    {
       public const string Code = "Meeting";
    }
}
