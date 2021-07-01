using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;

namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Công văn đi cần theo dõi", IsActive = true, IsShow = true, Position = 3, Icon = "PackageGo", Parent = DispatchGoMenuGroupController.Code)]
    public class DispatchGoCCController : BaseController
    {
        //
        // GET: /Dispatch/DispatchGoCC/
        private DispatchGoSV dispatchGoSV = new DispatchGoSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadDispatchGoCC(StoreRequestParameters parameters, int focusId = 0)
        {
            int us = User.EmployeeID;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = dispatchGoSV.GetCC(filter, us, User.ID, focusId);
            return this.Store(lst, filter.PageTotal);
        }
    }
}
