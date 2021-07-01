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

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thống kê đánh giá KPIs", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 6, Parent = GroupAuditKPIController.Code)]
    public class AuditKPIStatisticalController : BaseController
    {
        //
        // GET: /Human/AuditKPIStatistical/
        private HumanAuditGradationSV humanAuditGradationSV = new HumanAuditGradationSV();
        private HumanAuditGradationRoleSV humanAuditGradationRoleSV = new HumanAuditGradationRoleSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int humanAuditGradationId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditGradationRoleStatisticalItem> lstData;
            lstData = humanAuditGradationRoleSV.GetAllStatistical(humanAuditGradationId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult ListResult(int HumanAuditGradationRoleID,int AuditLevelID)
        {
            try
            {
                ViewData["HumanAuditGradationRoleID"] = HumanAuditGradationRoleID;
                ViewData["AuditLevelID"] = AuditLevelID;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult GetListData(StoreRequestParameters parameters, int HumanAuditGradationRoleID = 0, int AuditLevelID = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditTickItem> lstData;
            lstData = humanAuditGradationRoleSV.GetResultByLevel(filter, HumanAuditGradationRoleID, AuditLevelID);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult LoadDataPlan(StoreRequestParameters parameters, string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = System.Convert.ToDateTime(fromDate);
            }
            else
            {
                searchFromDate = DateTime.Now;
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = System.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            else
            {
                searchToDate = DateTime.Now;
            }
            List<HumanAuditGradationItem> lst;
            int total;
            lst = humanAuditGradationSV.GetPlanByDate(parameters.Page, parameters.Limit, out total, searchFromDate, searchToDate);
            return this.Store(new Paging<HumanAuditGradationItem>(lst, total));
        
        }
    }
}
