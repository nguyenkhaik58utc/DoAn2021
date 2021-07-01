using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Đánh giá", Icon = "UserComment", IsActive = true,IsGroup = true, IsShow = true, Position = 4, Parent = GroupAuditKPIController.Code)]
    public class GroupAuditPerformController : BaseController
    {
        public const string Code = "GroupAuditPerform";
    }
}
