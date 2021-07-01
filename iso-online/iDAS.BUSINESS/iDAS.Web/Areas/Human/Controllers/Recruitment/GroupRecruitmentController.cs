using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Tuyển dụng nhân sự", IsActive = true, Icon ="UserEarth", IsShow = true, Position = 3, IsGroup = true)]
    public class GroupRecruitmentController : BaseController
    {
        public const string Code = "GroupRecruitment";
    }
}
