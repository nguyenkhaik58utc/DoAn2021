using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Util.Controllers
{
    [BaseSystem(Name = "Đánh giá năng lực", Icon = "User", IsActive = true, IsShow = true, Position = 4, IsGroup = true)]
    public class GroupAuditEmployeeController : BaseController
    {
        public const string Code = "GroupAuditEmployee";
    }
}
