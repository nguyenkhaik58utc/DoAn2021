using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize =false)]
    public class CustomerCareResultController : BaseController
    {
        private CustomerCareResultSV CustomerCareResultService = new CustomerCareResultSV();
        public ActionResult CareForm(int id = 0, int groupCustomerID = 0)
        {
            var data = new CustomerCareResultItem();
            data.CustomerID = id;
            data.GroupCustomerID = groupCustomerID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Care", Model = data };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerCare(StoreRequestParameters parameters, int customerID = 0)
        {
            int totalCount;
            return this.Store(CustomerCareResultService.GetByCustomer(parameters.Page, parameters.Limit, out totalCount, customerID), totalCount);
        }
        public ActionResult LoadCare(StoreRequestParameters parameters, int customerID = 0)
        {
            var CustomerCareServices = new CustomerCareSV();
            int totalCount;
            return this.Store(CustomerCareServices.GetByCustomerID(parameters.Page, parameters.Limit, out totalCount, customerID), totalCount);
        }
        public ActionResult CareUpdateForm(int customerID = 0, int groupCustomerID = 0, int careResultID = 0)
        {
            var data = new CustomerCareResultItem();

            if (careResultID != 0)
            {
                data = CustomerCareResultService.GetById(careResultID);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ID = careResultID;
                data.CustomerID = customerID;
                data.GroupCustomerID = groupCustomerID;
                data.ActionForm = Form.Add;

            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "CareUpdate", Model = data };
        }
        /// <returns></returns>
        public ActionResult CareUpdateAction(CustomerCareResultItem data)
        {
            try
            {
                if (data.ID == 0 || data.ID == null)
                {
                    CustomerCareResultService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    CustomerCareResultService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winCareUpdate").Close();
                X.GetCmp<Store>("StoreCustomerCare").Reload();
            }
            return this.Direct();
        }
        public ActionResult DeleteCare(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    CustomerCareResultService.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerCare").Reload();
            }
            return this.Direct();
        }
        public ActionResult CareDetailForm(int careResultID = 0)
        {
            var data = new CustomerCareResultItem();
            if (careResultID != 0)
            {
                data = CustomerCareResultService.GetById(careResultID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "CareDetail", Model = data };
        }
   }
}
