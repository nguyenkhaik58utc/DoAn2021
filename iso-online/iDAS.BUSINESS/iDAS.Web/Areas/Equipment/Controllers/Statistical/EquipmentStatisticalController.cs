using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Equipment.Controllers
{
    [BaseSystem(Name = "Thống kê", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 5, IsGroup = true)]
    public class EquipmentStatisticalController : BaseController
    {
        public const string Code = "EquipmentStatistical";
    }
}
