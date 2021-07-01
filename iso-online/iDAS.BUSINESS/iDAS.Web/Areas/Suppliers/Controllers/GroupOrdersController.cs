using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [BaseSystem(Name = "Quản lý mua hàng", Icon = "ApplicationForm", IsActive = true, IsShow = true, Position = 4, IsGroup = true)]
    public class GroupOrdersController : BaseController
    {
        public const string Code = "GroupOrders";

    }
}
