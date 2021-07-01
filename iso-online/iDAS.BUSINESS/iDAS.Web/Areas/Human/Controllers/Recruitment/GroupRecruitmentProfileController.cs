using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Hồ sơ tuyển dụng", IsActive = true, Icon = "User", IsShow = true, Position = 4, IsGroup = true, Parent = GroupRecruitmentController.Code)]
    public class GroupRecruitmentProfileController : BaseController
    {
        public const string Code = "GroupRecruitmentProfile";
    }
}
