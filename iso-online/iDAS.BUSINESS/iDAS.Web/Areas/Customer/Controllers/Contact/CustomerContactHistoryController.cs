using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerContactHistoryController : BaseController
    {
        private CustomerContactHistorySV CustomerContactHistoryService = new CustomerContactHistorySV();
        public ActionResult Index(int id, bool isPotential = false, bool isOfficial = false )
        {
            CustomerContactHistoryItem data = new CustomerContactHistoryItem();
            data.CustomerID = id;
            data.IsPotential = isPotential;
            data.IsOfficial = isOfficial;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Index", Model = data };
        }
        /// <summary>
        /// Lấy danh sách 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerContactHistory(StoreRequestParameters parameters, int customerID = 0)
        {
            int totalCount;
            return this.Store(CustomerContactHistoryService.GetByCustomer(parameters.Page, parameters.Limit, out totalCount, customerID), totalCount);
        }
        #region Cập nhật Liên hệ
        public ActionResult UpdateForm(int ID = 0, int customerID = 0, bool isPotential = false, bool isOfficial = false)
        {
            var data = new CustomerContactHistoryItem();
            data.ActionForm = Form.Add;
            data.IsPotential = isPotential;
            data.IsOfficial = isOfficial;
            data.CustomerID = customerID;
            if (ID != 0)
            {
                data = CustomerContactHistoryService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.Employee = CustomerContactHistoryService.GetEmployeeByID(User.EmployeeID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerContactHistoryItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerContactHistoryService.Update(data, User.ID, User.EmployeeID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerContactHistoryService.Insert(data, User.ID, User.EmployeeID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winCustomerContactHistoryUpdate").Close();
                X.GetCmp<Store>("StoreCustomerContactHistory").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        public ActionResult Delete(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerContactHistoryService.Delete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerContactHistory").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerContactHistoryItem();
            if (ID != 0)
            {
                data = CustomerContactHistoryService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}