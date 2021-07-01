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
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý chăm sóc", IsActive = true, Icon = "UserStar", IsShow = true, Position = 3, Parent = GroupCommonController.Code)]
    public class CustomerCareController : BaseController
    {
        private CustomerCareSV CustomerCareService = new CustomerCareSV();
        private CustomerCareObjectSV CustomerCareObjectService = new CustomerCareObjectSV();
        private CustomerCareResultSV CustomerCareResultService = new CustomerCareResultSV();
         [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadCustomerCare(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = CustomerCareService.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }
        #region Cập nhật chăm sóc
         [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateForm(int ID = 0)
        {
            var data = new CustomerCareItem();

            if (ID != 0)
            {
                data = CustomerCareService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
                data.StatusValue = CustomerStatus.New;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerCareItem data)
        {
            try
            {
                data.IsPause = data.StatusValue == CustomerStatus.Pause ? true : false;
                data.IsFinish = data.StatusValue == CustomerStatus.Finish ? true : false;
                if (data.ID != 0)
                {
                    CustomerCareService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerCareService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winUpdateCustomerCare").Close();
                X.GetCmp<Store>("StoreCustomerCare").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa chăm sóc
         [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerCareService.Delete(ID);
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
        #endregion

        #region Xem chi tiết chăm sóc
         [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerCareItem();
            if (ID != 0)
            {
                data = CustomerCareService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Đối tượng chăm sóc khách hàng
        public ActionResult ObjectForm(int ID = 0)
        {
            var data = new CustomerCareItem();
            data.ID = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Object", Model = data };
        }
        /// <summary>
        /// Lấy danh sách đối tượng khách hàng được chăm sóc
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerCareObject(StoreRequestParameters parameters, int careID = 0)
        {
            int totalCount;
            var result = CustomerCareObjectService.GetAllCustomerCareObject(parameters.Page, parameters.Limit, out totalCount, careID);
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateCustomerCareObject(string data)
        {
            var CustomerCareObjectItem = Ext.Net.JSON.Deserialize<CustomerCareObjectItem>(data);
            try
            {
                if (CustomerCareObjectItem.ID == null && CustomerCareObjectItem.IsSelect == true)
                {
                    CustomerCareObjectService.Insert(CustomerCareObjectItem, User.ID);

                }
                if (CustomerCareObjectItem.ID != null && CustomerCareObjectItem.IsSelect == false)
                {
                    CustomerCareObjectService.Delete((int)CustomerCareObjectItem.ID);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            X.GetCmp<Store>("storeCustomerCareObject").Reload();
            return this.Direct();
        }
        #endregion

        #region Kết quả chăm sóc khách hàng
        public ActionResult LoadResultCustomerCareObject(StoreRequestParameters parameters, int careID = 0)
        {
            int totalCount;
            var result = CustomerCareObjectService.GetCustomerCareObject(parameters.Page, parameters.Limit, out totalCount, careID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// Lấy các khách hàng đã được chăm sóc theo nhóm
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadCaredCustomerByGroup(StoreRequestParameters parameters, int groupID = 0, int careID = 0)
        {
            int totalCount;
            var result = CustomerCareResultService.GetByGroupIdAndCareId(parameters.Page, parameters.Limit, out totalCount, groupID, careID);
            return this.Store(result, totalCount);
        }
        public ActionResult ResultForm(int ID = 0)
        {
            var data = new CustomerCareItem();
            data.ID = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Result", Model = data };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ResultUpdateForm(int customerID = 0, int careID = 0, string customerName = "")
        {
            var data = new CustomerCareResultItem();

            if (customerID != 0 && careID != 0)
            {
                var result = CustomerCareResultService.GetByCareIdAndCustomerId(careID, customerID);
                if (result != null)
                {
                    data = result;
                    data.ActionForm = Form.Edit;
                }
                else
                {
                    data.CustomerID = customerID;
                    data.CareID = careID;
                    data.Name = customerName;
                    data.ActionForm = Form.Add;

                }
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ResultUpdate", Model = data };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult UpdateCustomerCareResult(CustomerCareResultItem data)
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
                X.GetCmp<Window>("winUpdateCustomerCareResult").Close();
            }
            return this.Direct();
        }
        /// <summary>
        /// Xem chi tiết kết quả
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="careID"></param>
        /// <returns></returns>
        public ActionResult ResultCustomerDetailForm(int customerID = 0, int careID = 0)
        {
            var result = CustomerCareResultService.GetByCareIdAndCustomerId(careID, customerID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ResultCustomerDetail", Model = result };
        }
        #endregion

    }
}
