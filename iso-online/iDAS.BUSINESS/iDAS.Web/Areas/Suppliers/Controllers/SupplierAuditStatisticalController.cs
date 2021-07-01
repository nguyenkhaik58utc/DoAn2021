using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [BaseSystem(Name = "Thống kê kế hoạch đánh giá", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 5)]
    public class SupplierAuditStatisticalController : BaseController
    {
        //
        // GET: /Suppliers/SupplierAuditStatistical/
        SupplierAuditPlanSV suppAuditPlanSV = new SupplierAuditPlanSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters,string fromDate, string toDate)
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
            List<SupplierAuditPlanItem> lstData;
            int total;
            lstData = suppAuditPlanSV.GetAllApprove(parameters.Page, parameters.Limit, out total, searchFromDate, searchToDate);
            return this.Store(new Paging<SupplierAuditPlanItem>(lstData, total));
            
        }

        public ActionResult ViewDetailStatistical(string urlStore = "", StoreParameter param = null,int planID=0)
        {
            ViewData["StoreUrlProfile"] = urlStore;
            ViewData["StoreParamProfileStatiscal"] = param;            
            ViewData["PlanID"] = planID;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        public ActionResult renderTotalAuditChecked(StoreRequestParameters parameters, int planID = 0)
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);                
                List<SupplierAuditItem> lst = new List<SupplierAuditItem>();
                lst = new SupplierAuditSV().GetAllByPlanIdChecked(filter.PageIndex,filter.PageSize,out filter.PageTotal, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalAuditNotCheck(StoreRequestParameters parameters, int planID = 0)
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<SupplierAuditItem> lst = new List<SupplierAuditItem>();
                lst = new SupplierAuditSV().GetAllByPlanIdNotChecked(filter.PageIndex, filter.PageSize, out filter.PageTotal, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalPass(StoreRequestParameters parameters, int planID = 0)
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<SupplierAuditItem> lst = new List<SupplierAuditItem>();
                lst = new SupplierAuditSV().GetAllByPlanIdPass(filter.PageIndex, filter.PageSize, out filter.PageTotal, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalNotPass(StoreRequestParameters parameters, int planID = 0)
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<SupplierAuditItem> lst = new List<SupplierAuditItem>();
                lst = new SupplierAuditSV().GetAllByPlanIdNotPass(filter.PageIndex, filter.PageSize, out filter.PageTotal, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalAudit(StoreRequestParameters parameters, int planID = 0)
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<SupplierAuditItem> lst = new List<SupplierAuditItem>();
                lst = new SupplierAuditSV().GetAllByPlanId(filter.PageIndex, filter.PageSize, out filter.PageTotal, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
    }
}
