using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [BaseSystem(Name = "Quản Lý Chiến Lược", IsActive = true, Icon = "PageEdit", IsShow = true, Position = 2, IsGroup = true)]
    public class GroupCommonController : BaseController
    {
        public const string Code = "GroupCommon";
    }
}
