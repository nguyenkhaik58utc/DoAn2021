using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Equipment.Controllers
{
    [BaseSystem(Name = "Danh Mục Nhóm Thiết Bị", IsActive = true, Icon = "Group", IsShow = true, Position = 1, IsGroup = true)]
    public class GroupCategoryController : BaseController
    {
        public const string Code = "GroupCategory";
    }
}
