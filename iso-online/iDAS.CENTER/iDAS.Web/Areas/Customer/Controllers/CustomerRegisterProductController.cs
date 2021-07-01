using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using System.Text;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Customer.Controllers
{
    [BaseSystem(Name = "Đăng ký sản phẩm", IsActive = true, IsShow = true, Position = 1, Icon = "UserMagnify", Parent = "GroupCustomerRegister")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerRegisterProductController : BaseController
    {
        private CustomerRegisterProductSV service = new CustomerRegisterProductSV();

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
                var data = service.GetCustomerRegisterProducts(parameters.Page, parameters.Limit, out total);
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
        public ActionResult LoadProducts(int systemId = 0)
        {
            try
            {
                var data = service.GetProducts(systemId);
                return this.Store(data);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadProductMethods()
        {
            try
            {
                var data = service.GetProductMethods();
                return this.Store(data);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadProductDatasizes()
        {
            try
            {
                var data = service.GetProductDataSizes();
                return this.Store(data);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadProductScopes()
        {
            try
            {
                var data = service.GetProductScopes();
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
        [ValidateInput(false)]
        public ActionResult Create(CustomerRegisterProductItem item)
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
        public ActionResult Update(int CustomerRegisterProductId)
        {
            try
            {
                var data = service.GetCustomerRegisterProduct(CustomerRegisterProductId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(CustomerRegisterProductItem item)
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
        public ActionResult Delete(int CustomerRegisterProductId = 0)
        {
            try
            {
                service.Delete(CustomerRegisterProductId);
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
        public ActionResult Detail(int CustomerRegisterProductId)
        {
            try
            {
                var data = service.GetCustomerRegisterProduct(CustomerRegisterProductId);
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
