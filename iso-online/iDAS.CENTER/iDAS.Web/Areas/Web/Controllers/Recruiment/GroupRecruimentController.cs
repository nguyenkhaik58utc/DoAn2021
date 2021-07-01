using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Quản lý tuyển dụng", IsActive = true, IsShow = true, IsGroup = true, Position = 4, Icon = "PageWhite")]
    public class GroupRecruimentController : BaseController
    {
        public const string Code = "GroupRecruiment";
    }
}
