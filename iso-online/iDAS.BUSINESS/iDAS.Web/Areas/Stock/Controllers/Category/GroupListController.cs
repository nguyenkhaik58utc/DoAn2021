
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using System.Reflection;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Quản lý danh mục chung", Icon = "PageGear", IsActive = true, IsShow = true, Position = 1, IsGroup = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class GroupListController : BaseController
    {
        //
        // GET: /Stock/GroupList/
        public const string Code = "GroupList";
    }
    [BaseSystem(Name = "Quản lý kho", IsActive = true, IsShow = true, Position = 1,Icon="HouseStar", IsGroup = true)]
    public class GroupManageStockController : BaseController
    {
        //
        // GET: /Stock/GroupList/
        public const string Code = "GroupManageStock";
    }
}
