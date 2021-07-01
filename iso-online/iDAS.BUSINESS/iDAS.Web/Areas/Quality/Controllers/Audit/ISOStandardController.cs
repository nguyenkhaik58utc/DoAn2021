using Ext.Net;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;


namespace iDAS.Web.Areas.Quality.Controllers
{

    [BaseSystem(Name = "Danh sách tiêu chuẩn ISO", IsActive = true, Icon = "PageGear", IsShow = true, Position = 1, Parent = GroupMenuAuditController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ISOStandardController : BaseController
    {
        //
        // GET: /Task/CriteriaCategory/
        private ISOStandardSV iSOStandardSV = new ISOStandardSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FormSetCateCriteria(int id = 0)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "SetCateCriteria", ViewData = new ViewDataDictionary { { "iSOStandardId", id } } };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult DetailForm(int id)
        {
            var data = iSOStandardSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            List<CenterISOStandardItem> lstData;
            int total;
            lstData = iSOStandardSV.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(lstData, total);
        }
    }
}
