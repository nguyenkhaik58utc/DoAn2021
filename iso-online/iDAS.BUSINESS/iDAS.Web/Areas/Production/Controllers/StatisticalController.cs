using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
namespace iDAS.Web.Areas.Production.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thống kê", IsActive = true, IsShow = true, Position = 6, Icon = "ChartBar")]
    public class StatisticalController : BaseController
    {
        private EquipmentProductionSV ProductionServices = new EquipmentProductionSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
