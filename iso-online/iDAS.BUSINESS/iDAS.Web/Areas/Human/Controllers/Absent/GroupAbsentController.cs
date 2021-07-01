using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers.Absent
{
    [BaseSystem(Name = "Nghỉ phép", Icon = "User", IsActive = false, IsShow = false, Position = 6, IsGroup = true)]
    public class GroupAbsentController : BaseController
    {
        public const string Code = "GroupAbsent";

    }
}
