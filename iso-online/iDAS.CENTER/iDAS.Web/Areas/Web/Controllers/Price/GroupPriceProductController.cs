using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Bảng giá sản phẩm", IsActive = true, IsShow = true, IsGroup = true, Position = 1, Icon = "PackageStart", Parent = GroupPriceController.Code)]
    public class GroupPriceProductController : BaseController
    {
        public const string Code = "GroupPriceProduct";
    }
}
