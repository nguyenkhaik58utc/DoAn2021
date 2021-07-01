using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human.Controllers
{
    [BaseSystem(Name = "Thống kê đào tạo nhân sự", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 3, Parent = GroupStatisticalController.Code)]
    public class TrainingEmployeeStatistiaclController : BaseController
    {
        //
        // GET: /Human/TrainingEmployeeStatistiacl/
        HumanTrainingStaticticalSV statisticalSV = new HumanTrainingStaticticalSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int planID)
        {

            List<HumanTrainingPlanDetailItem> lstData;
            int total;
            lstData = new HumanTrainingPlanDetailSV().GetAllStatistiacl(parameters.Page, parameters.Limit, out total, planID);
           
            return this.Store(new Paging<HumanTrainingPlanDetailItem>(lstData, total));
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
            List<HumanTrainingPlanItem> lst;
            int total;
            lst = new HumanTrainingPractionersSV().GetPlanIsAcceptByDate(parameters.Page, parameters.Limit, out total,searchFromDate,searchToDate);
            return this.Store(new Paging<HumanTrainingPlanItem>(lst, total));
        }
    }
}
