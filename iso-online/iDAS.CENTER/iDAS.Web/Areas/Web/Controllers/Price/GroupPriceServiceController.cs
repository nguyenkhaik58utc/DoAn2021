using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Bảng giá dịch vụ", IsActive = true, IsShow = true, IsGroup = true, Position = 2, Icon = "DeviceStylus", Parent = GroupPriceController.Code)]
    public class GroupPriceServiceController : BaseController
    {
        public const string Code = "GroupPriceService";
    }
}
