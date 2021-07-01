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

namespace iDAS.Web.Areas.Util.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kết quả", Icon = "StarGold", IsActive = true, IsShow = true, Position = 4, Parent = GroupAuditEmployeeController.Code)]
    public class ResultAuditController : BaseController
    {
        //
        // GET: /Human/ResultAudit/
        private HumanPhaseAuditSV humanPhaseAuditSV = new HumanPhaseAuditSV();
        private HumanResultQuestionSV humanResultQuestionSV = new HumanResultQuestionSV();
        private HumanEmployeeAuditSV humanEmployeeAuditSV = new HumanEmployeeAuditSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanPhaseAuditItem> lstData;
            lstData = humanPhaseAuditSV.GetIsApproval(filter);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataEmployeeAudit(StoreRequestParameters parameters, int phaseAuditId=0)
        {
            List<HumanEmployeeAuditItem> lstData;
            int total;
            lstData = humanEmployeeAuditSV.GetAll(parameters.Page, parameters.Limit, out total, phaseAuditId);
            return this.Store(new Paging<HumanEmployeeAuditItem>(lstData, total));
        }
        public ActionResult ShowDetailAnswer(int employeeId = 0, int phaseAuditID=0)
        {
            if (employeeId > 0)
            {
                var humanEmployeeAuditId = humanEmployeeAuditSV.GetByPhaseAuditIDAndEmployeeID(phaseAuditID, employeeId);
                ViewData["EmployeeID"] = employeeId;
                ViewData["HumanEmployeeAuditId"] = humanEmployeeAuditId;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailAnswer", ViewData = ViewData};
            }
            return null;
        }
        public ActionResult GetQuestionAnswer(StoreRequestParameters parameters, int cateId = 0, int employeeId=0, int humanEmployeeAuditID=0)
        {
            List<HumanResultQuestionItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = humanResultQuestionSV.GetResultQuestion(filter, cateId, employeeId, humanEmployeeAuditID);
            return this.Store(lstData, filter.PageTotal);
        }
    }
}
