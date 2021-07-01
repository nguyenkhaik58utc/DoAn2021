using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Items;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [BaseSystem(Name = "Đăng ký dung lượng", IsActive = true, IsShow = true, Position = 3, StartAction = "Index", Icon = "ApplicationOsxGo", Parent = "GroupCustomerRegister")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerRegisterDatasizeController : BaseController
    {
        private CustomerRegisterDatasizeSV service = new CustomerRegisterDatasizeSV();

        #region View
        [BaseSystem(Name = "Xem", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var data = service.GetCustomerRegisterDatasizes(parameters.Page, parameters.Limit, out total);
                return this.Store(data, total);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadCustomers()
        {
            try
            {
                var data = service.GetCustomers();
                return this.Store(data);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadSystems()
        {
            try
            {
                var data = service.GetSystems();
                return this.Store(data);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        #region Create
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Create()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(CustomerRegisterDatasizeItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    service.Insert(item);
                    Ultilities.ShowMessageRequest(RequestStatus.CreateSuccess);
                    result = true;
                }
                else
                {
                    Ultilities.ShowMessageRequest(RequestStatus.ValidError);
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(result);
        }
        #endregion
        #region Update
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int CustomerRegisterDatasizeId)
        {
            try
            {
                var data = service.GetCustomerRegisterDatasize(CustomerRegisterDatasizeId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(CustomerRegisterDatasizeItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    service.Update(item);
                    Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                    result = true;
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(result);
        }
        #endregion
        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int CustomerRegisterDatasizeId = 0)
        {
            try
            {
                service.Delete(CustomerRegisterDatasizeId);
                Ultilities.ShowMessageRequest(RequestStatus.DeleteSuccess);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.DeleteError);
            }
            return this.Direct();
        }
        #endregion
        #region Detail
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Detail(int CustomerRegisterDatasizeId)
        {
            try
            {
                var data = service.GetCustomerRegisterDatasize(CustomerRegisterDatasizeId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion 
    }
}
