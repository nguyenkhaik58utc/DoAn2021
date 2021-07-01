using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;

namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Công văn đến cần theo dõi", IsActive = true, Icon = "PackageIn", IsShow = true, Position = 4, Parent = DispatchToMenuController.Code)]
    public class DispatchToCCController : BaseController
    {
        //
        // GET: /Dispatch/DispatchToCC/
        private DispatchToSV dispatchToSV = new DispatchToSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadDispatchToCC(StoreRequestParameters parameters, int focusId = 0)
        {
            int us = User.EmployeeID;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = dispatchToSV.GetCC(filter, us, focusId);
            return this.Store(lst, filter.PageTotal);
        }
    }
}
