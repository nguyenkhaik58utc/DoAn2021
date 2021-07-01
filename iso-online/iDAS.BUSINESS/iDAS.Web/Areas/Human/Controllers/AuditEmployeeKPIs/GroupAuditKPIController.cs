using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Đánh giá nhân sự KPIs", Icon = "User", IsActive = true, IsShow = true, Position = 5, IsGroup = true)]
    public class GroupAuditKPIController : BaseController
    {
        //
        // GET: /Human/GroupAuditKPI/
        public const string Code = "GroupAuditKPI";
    }
}
