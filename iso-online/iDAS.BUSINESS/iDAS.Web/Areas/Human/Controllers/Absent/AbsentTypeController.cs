using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Human.Controllers.Absent
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kiểu nghỉ phép", Icon = "UserComment", IsActive = false, IsShow = false, Position = 1, Parent = GroupAbsentController.Code)]
    public class AbsentTypeController : BaseController
    {
        //
        // GET: /Human/AbsentType/
        HumanAbsentTypeSV humanAbsentTypeSV = new HumanAbsentTypeSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAbsentTypeItem> lstData = new List<HumanAbsentTypeItem>();
            lstData = humanAbsentTypeSV.GetAll(filter);
            return this.Store(lstData);
        }
        
    }
}
