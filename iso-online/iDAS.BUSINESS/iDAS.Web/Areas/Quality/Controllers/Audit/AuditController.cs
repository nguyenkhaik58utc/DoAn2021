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

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Kết quả đánh giá", Icon = "ScriptEdit", IsActive = true, IsShow = true, Position = 4, Parent = GroupMenuAuditController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class AuditController : BaseController
    {
        //
        // GET: /Quality/AuditResult/
        private QualityAuditProgramISOIndexSV qualityAuditProgramISOIndexSV = new QualityAuditProgramISOIndexSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            var obj = qualityAuditProgramISOIndexSV.GetByID(id);
            return PartialView(obj);
        }
        public ActionResult Detail(int id)
        {
            var obj = qualityAuditProgramISOIndexSV.GetByID(id);
            return PartialView(obj);
        }
        public ActionResult Result(int iSOStandardId = 0)
        {
            return PartialView(ViewData = new ViewDataDictionary { { "iSOStandardId", iSOStandardId } });
        }
        public ActionResult DetailResult(int iSOStandardId = 0)
        {
            return PartialView(ViewData = new ViewDataDictionary { { "iSOStandardId", iSOStandardId } });
        }
        public ActionResult ResultAdd(int iSOStandardId = 0)
        {
            return PartialView(ViewData = new ViewDataDictionary { { "iSOStandardId", iSOStandardId } });
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AuditUpdateWindow(int id)
        {
            ViewData["id"] = id;
            return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AuditDetailWindow(int id)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewData = new ViewDataDictionary { { "id", id } } };
        }

        public ActionResult GetData(StoreRequestParameters parameters, int auditPlanId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var Task = qualityAuditProgramISOIndexSV.GetByPlan(filter, auditPlanId);
            return this.Store(Task, filter.PageTotal);
        }
        public ActionResult Update(QualityAuditProgramISOIndexItem objEdit)
        {
            try
            {
                qualityAuditProgramISOIndexSV.Update(objEdit, User.ID);
                X.GetCmp<Store>("stMnDetailResult").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                return this.Direct(true);
            }
            catch 
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct(false);
            }

        }
    }
}
