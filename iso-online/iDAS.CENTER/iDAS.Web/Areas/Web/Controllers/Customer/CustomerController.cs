using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Danh sách khách hàng", IsActive = true, IsShow = true, Icon = "UserComment", Position = 1, Parent = "GroupWebCustomer")]
    public class WebCustomerController : BaseController
    {
        private WebCustomerSV service = new WebCustomerSV();

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
                var data = service.GetWebCustomers(parameters.Page, parameters.Limit, out total);
                return this.Store(data, total);
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
        public ActionResult Create(WebCustomerItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!service.CheckExist(item))
                    {
                        service.Insert(item);
                        Ultilities.ShowMessageRequest(RequestStatus.CreateSuccess);
                        result = true;
                    }
                    else
                    {
                        Ultilities.ShowMessageRequest(RequestStatus.ExistError);
                    }
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
        public ActionResult Update(int webCustomerId)
        {
            try
            {
                var data = service.GetWebCustomer(webCustomerId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(WebCustomerItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!service.CheckExist(item))
                    {
                        service.Update(item);
                        Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                        result = true;
                    }
                    else
                    {
                        Ultilities.ShowMessageRequest(RequestStatus.ExistError);
                    }
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
        public ActionResult Delete(int webCustomerId = 0)
        {
            try
            {
                service.Delete(webCustomerId);
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
        public ActionResult Detail(int webCustomerId)
        {
            try
            {
                var data = service.GetWebCustomer(webCustomerId);
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
