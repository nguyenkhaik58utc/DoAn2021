using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Production.Controllers
{
    [BaseSystem(Name = "Thiết lập", IsActive = true, Icon = "TagBlue", IsShow = true, Position = 1, IsGroup = true)]
    public class GroupCategoryController : BaseController
    {
        public const string Code = "GroupCategory";
    }
}
