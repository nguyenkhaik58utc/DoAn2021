using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    public class CustomerCampaignAuditController : BaseController
    {
        private CustomerCampaignAuditSV customerAuditService = new CustomerCampaignAuditSV();

        public ActionResult LoadAudits(StoreRequestParameters parameters, int campaignId = 0)
        {
           // int total;
           //var lstData = customerAuditService.GetAll(parameters.Page, parameters.Limit, out total, campaignId);
           // return this.Store(lstData, total);
            return View();
        }
        public ActionResult Insert(AuditItem audit, int campaignId = 0)
        {
            var success = false;
            try
            {
               customerAuditService.Insert(audit, campaignId);
                success = true;
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }
    }
}
