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
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerContractController : BaseController
    {
        private CustomerContractSV CustomerContractService = new CustomerContractSV();
        private CustomerOrderSV CustomerOrderService = new CustomerOrderSV();
        public ActionResult Index()
        {
            return View();
        }

        #region Hợp đồng cho khách hàng
        public ActionResult List(int id = 0)
        {
            CustomerContractItem data = new CustomerContractItem();
            data.CustomerID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "List", Model = data };
        }
        /// <summary>
        /// Lấy danh sách hợp đồng theo khách hàng
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerContract(StoreRequestParameters parameters, int customerID = 0)
        {
            int totalCount;
            return this.Store(CustomerContractService.GetByCustomer(parameters.Page, parameters.Limit, out totalCount, customerID), totalCount);
        }
        #region Cập nhật hợp đồng
        public ActionResult UpdateForm(int ID = 0, int customerID = 0)
        {
            var data = new CustomerContractItem();
            data.ActionForm = Form.Add;
            data.CustomerID = customerID;
            if (ID != 0)
            {
                data = CustomerContractService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerContractItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerContractService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerContractService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winUpdateCustomerContract").Close();
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            return this.Direct(1);
        }
        #endregion

        #region Xóa hợp đồng
        public ActionResult Delete(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerContractService.Delete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết hợp đồng
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                data = CustomerContractService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Thiết lập đơn hàng
        public ActionResult OrderForm(int customerId = 0, int contractId = 0)
        {
            var data = new CustomerOrderSelected();
            data.CustomerID = customerId;
            data.ContractID = contractId;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Order", Model = data };
        }
        public ActionResult LoadCustomerOrder(StoreRequestParameters parameters, int customerId = 0, int contractId = 0)
        {
            int totalCount;
            var result = CustomerOrderService.GetContractSelect(parameters.Page, parameters.Limit, out totalCount, contractId, customerId);
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateOrder(string data, int contractId = 0)
        {
            try
            {
                var OrderItem = Ext.Net.JSON.Deserialize<CustomerOrderSelected>(data);
                if (OrderItem.ID != 0)
                {
                    if (contractId > 0 && OrderItem.IsSelect)
                    {
                        OrderItem.ContractID = contractId;
                    }
                    CustomerOrderService.UpdateContract(OrderItem);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion
        #endregion
    }
}
