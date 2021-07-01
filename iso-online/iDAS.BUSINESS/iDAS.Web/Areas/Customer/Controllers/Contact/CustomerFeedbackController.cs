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
    [SystemAuthorize(CheckAuthorize=false)]
    public class CustomerFeedbackController : BaseController
    {
        private CustomerFeedbackSV CustomerFeedbackService = new CustomerFeedbackSV();
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int id)
        {
            CustomerFeedbackItem data = new CustomerFeedbackItem();
            data.CustomerID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Index", Model = data };
        }
        /// <summary>
        /// Lấy danh sách Đơn hàng
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerFeedback(StoreRequestParameters parameters, int customerID = 0)
        {
            int totalCount;
            return this.Store(CustomerFeedbackService.GetByCustomer(parameters.Page, parameters.Limit, out totalCount, customerID), totalCount);
        }
        #region Cập nhật 
        public ActionResult UpdateForm(int ID = 0, int customerID=0)
        {
            var data = new CustomerFeedbackItem();
            data.ActionForm = Form.Add;
            data.CustomerID = customerID;
            if (ID != 0)
            {
                data = CustomerFeedbackService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerFeedbackItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerFeedbackService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerFeedbackService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winCustomerFeedbackCustomer").Close();
                X.GetCmp<Store>("StoreCustomerFeedback").Reload();
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
                    CustomerFeedbackService.Delete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerFeedback").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerFeedbackItem();
            if (ID != 0)
            {
                data = CustomerFeedbackService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

    }
}
