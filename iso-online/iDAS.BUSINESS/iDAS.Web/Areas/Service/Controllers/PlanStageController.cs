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

namespace iDAS.Web.Areas.Service.Controllers
{
    public class PlanStageController : BaseController
    {
        //
        // GET: /Service/PlanPhase/
        private ServicePlanStageSV planPhaseSV = new ServicePlanStageSV();
        public ActionResult GetData(StoreRequestParameters parameters, int serviceId, int planId)
        {
            List<ServicePlanStageItem> lst;
            int total;
            lst = planPhaseSV.GetAllByServiceID(parameters.Page, parameters.Limit, out total, serviceId, planId);
            return this.Store(new Paging<ServicePlanStageItem>(lst, total));
        }
        public ActionResult GetDataIsChoise(StoreRequestParameters parameters, int parentId)
        {
            List<ServicePlanItem> lst;
            int total;
            lst = planPhaseSV.GetAllByPlanParentIDIsChoice(parameters.Page, parameters.Limit, out total, parentId);
            return this.Store(new Paging<ServicePlanItem>(lst, total));
        }
        public ActionResult GetDataIsContractID(StoreRequestParameters parameters, int contractId)
        {
            List<ServicePlanItem> lst;
            int total;
            lst = planPhaseSV.GetForAccounting(parameters.Page, parameters.Limit, out total, contractId);
            return this.Store(new Paging<ServicePlanItem>(lst, total));
        }
        public ActionResult InsertPlanStage(string data, int servicePlanId, int customerId)
        {
            bool success = false;
            try
            {
                var criteria = Ext.Net.JSON.Deserialize<ServiceStageItem>(data);
                var obj = planPhaseSV.CheckExits(servicePlanId, customerId, criteria.ID);
                if (obj == 0 && criteria.IsSelected)
                {
                    planPhaseSV.Insert(servicePlanId, criteria.ID, customerId, User.ID);
                    Ultility.ShowNotification(message: RequestMessage.CreateSuccess);
                }
                if (obj != 0 && !criteria.IsSelected)
                {
                    planPhaseSV.Delete(obj);
                }
                X.GetCmp<Store>("StorePlanInsertPlanDetail").Reload();
                success = true;
            }
            catch
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.Direct(success);
        }
    }
}
