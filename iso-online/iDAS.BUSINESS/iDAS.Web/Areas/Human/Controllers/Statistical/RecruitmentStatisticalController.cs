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

namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Thống kê tuyển dụng nhân sự", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 2, Parent = GroupStatisticalController.Code)]
    public class RecruitmentStatisticalController : BaseController
    {
        //
        // GET: /Human/RecruitmentStatistical/
        private HumanRecruitmentStatisticalSV statisticalSV = new HumanRecruitmentStatisticalSV();
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
            List<HumanRecruitmentPlanItem> lstData;
            int total;
            lstData = statisticalSV.GetAllApprove(parameters.Page, parameters.Limit, out total, searchFromDate, searchToDate);
            return this.Store(new Paging<HumanRecruitmentPlanItem>(lstData, total));
        }
        public ActionResult ViewListProfile(int planID, string urlStore = "", StoreParameter param = null, string fromDate = "", string toDate = "")
        {
            ViewData["StoreUrlProfile"] = urlStore;
            ViewData["StoreParamProfileStatiscal"] = param;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            ViewData["planId"] = planID;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        public ActionResult renderTotalProfile(StoreRequestParameters parameters, int planID, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<RecruitmentProfileIneterviewSelectItem> lst = new List<RecruitmentProfileIneterviewSelectItem>();
                lst = new HumanRecruitmentProfileSV().GetByPlanID(filter, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalInterview(StoreRequestParameters parameters, int planID, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<RecruitmentProfileIneterviewSelectItem> lst = new List<RecruitmentProfileIneterviewSelectItem>();
                lst = new HumanRecruitmentProfileSV().GetByPlanIDHasResult(filter, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalTrial(StoreRequestParameters parameters, int planID, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<RecruitmentProfileIneterviewSelectItem> lst = new List<RecruitmentProfileIneterviewSelectItem>();
                lst = new HumanRecruitmentProfileSV().GetByPlanIDTrial(filter, planID);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalPass(StoreRequestParameters parameters, int planID, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
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
                List<RecruitmentProfileIneterviewSelectItem> lst = new List<RecruitmentProfileIneterviewSelectItem>();
                lst = new HumanRecruitmentProfileSV().GetByPlanIDPass(filter, planID);
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
