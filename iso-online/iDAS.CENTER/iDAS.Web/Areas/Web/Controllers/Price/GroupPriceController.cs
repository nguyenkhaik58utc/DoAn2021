using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Quản lý bảng giá", IsActive = true, IsShow = true, IsGroup = true, Position = 7, Icon = "MoneyDollar")]
    public class GroupPriceController : BaseController
    {
        public const string Code = "GroupPrice";
    }
}
