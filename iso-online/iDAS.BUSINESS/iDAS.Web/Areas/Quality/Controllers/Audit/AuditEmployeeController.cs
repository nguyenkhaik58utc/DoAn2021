using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    public class AuditEmployeeController : BaseController
    {
        //
        // GET: /Quality/AuditObject/
        private QualityAuditEmployeeSV auditObjectSV = new QualityAuditEmployeeSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int auditProgramId)
        {
            List<QualityAuditEmployeeItem> lstData;
            int total;
            lstData = auditObjectSV.GetAll(parameters.Page, parameters.Limit, out total, auditProgramId);
            return this.Store(new Paging<QualityAuditEmployeeItem>(lstData, total));
        }
        public ActionResult ChoiceObject(int auditProgramId = 0)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ChoiceObject", ViewData = new ViewDataDictionary { { "auditProgramId", auditProgramId } } };
        }
        public ActionResult ChoiceObjectAdd(int auditProgramId = 0)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ChoiceObjectAdd", ViewData = new ViewDataDictionary { { "auditProgramId", auditProgramId } } };
        }
        public ActionResult ShowFrmUpdate(int id)
        {
            var obj = auditObjectSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Update", Model = obj };
        }
        
       
    }
}
