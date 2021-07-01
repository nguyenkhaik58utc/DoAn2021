using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Quản lý đánh giá nội bộ", Icon = "ScriptEdit", IsActive = true, IsShow = true, Position = 3, IsGroup = true)]
    public class GroupMenuAuditController : BaseController
    {
        //
        // GET: /Quality/GroupMenuAudit/
        public const string Code = "GroupMenuAudit";

    }
}
