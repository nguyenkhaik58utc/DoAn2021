using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace iDAS.Web.Areas.Profile.Controllers
{
    [BaseSystem(Name = "Quản lý danh mục", IsActive = true, Icon = "PageLink", IsShow = true, Position = 1, IsGroup = true)]
    public class GroupCategoryController : BaseController
    {
        public const string Code = "GroupCategory";
    }
}
