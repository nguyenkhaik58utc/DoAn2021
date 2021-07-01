using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Customer.Controllers
{
    public class CustomerCampaignPlanController : BaseController
    {
        private CustomerCampaignPlanSV CustomerCampaignPlanSV = new CustomerCampaignPlanSV();
        public ActionResult LoadPlans(StoreRequestParameters parameters, int paramID = 0)
        {
            int totalCount;
            var Task = CustomerCampaignPlanSV.GetAll(parameters.Page, parameters.Limit, out totalCount, paramID);
            return this.Store(Task, totalCount);
        }
        public ActionResult Insert(QualityPlanItem Plan, int paramID = 0)
        {
            var success = false;
            try
            {
                    Plan.CreateBy = User.ID;
                    CustomerCampaignPlanSV.Insert(Plan, paramID);
                    success = true;
                    X.GetCmp<Store>("StorePlanForPartialView").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }    
    }
}
