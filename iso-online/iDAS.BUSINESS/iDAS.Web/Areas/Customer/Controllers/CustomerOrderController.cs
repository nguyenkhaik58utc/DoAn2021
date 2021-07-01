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
    public class CustomerOrderController : BaseController
    {
        private CustomerOrderSV CustomerOrderService = new CustomerOrderSV();
        public ActionResult Index(int Id)
        {
            CustomerOrderItem data = new CustomerOrderItem();
            data.CustomerID = Id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Index", Model = data };
        }
        /// <summary>
        /// Lấy danh sách Đơn hàng
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerOrder(StoreRequestParameters parameters, int customerID = 0)
        {
            int totalCount;
            return this.Store(CustomerOrderService.GetByCustomer(parameters.Page, parameters.Limit, out totalCount, customerID), totalCount);
        }
        #region Cập nhật Đơn
        public ActionResult UpdateForm(int ID = 0, int customerID = 0)
        {
            var data = new CustomerOrderItem();
            data.ActionForm = Form.Add;
            data.CustomerID = customerID;
            if (ID != 0)
            {
                data = CustomerOrderService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerOrderItem data)
        {
            try
            {
                data.IsPause = data.StatusValue == CustomerStatus.Pause ? true : false;
                data.IsFinish = data.StatusValue == CustomerStatus.Finish ? true : false;
                if (data.ID != 0)
                {
                    CustomerOrderService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerOrderService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winUpdateOrderCustomer").Close();
                X.GetCmp<Store>("StoreCustomerOrder").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa Đơn hàng
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    if (CustomerOrderService.Delete(ID))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    }
                    else
                    {
                        Ultility.CreateNotification(message: RequestMessage.DataUsing, error: true);
                    }
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerOrder").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết Đơn hàng
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerOrderItem();
            if (ID != 0)
            {
                data = CustomerOrderService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

    }
}
