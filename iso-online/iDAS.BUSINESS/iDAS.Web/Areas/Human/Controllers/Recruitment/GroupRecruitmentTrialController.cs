using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Hồ sơ thử việc", IsActive = true, Icon = "FolderUser", IsShow = true, Position = 5, IsGroup = true, Parent = GroupRecruitmentController.Code)]
    public class GroupRecruitmentTrialController : BaseController
    {
        public const string Code = "GroupRecruitmentTrial";
    }
}
