using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Risk.Controllers
{
    public class RisksController : BaseController
    {
        private CenterRiskCategorySV riskCategorySV = new CenterRiskCategorySV();
        public ActionResult GetDataCate()
        {
            var lst = riskCategorySV.GetAll();
            return this.Store(lst);
        }
    }
}
