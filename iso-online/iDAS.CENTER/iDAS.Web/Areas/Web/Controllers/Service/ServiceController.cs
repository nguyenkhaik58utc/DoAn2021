using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Core;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Danh sách dịch vụ", IsActive = true, IsShow = true, Icon = "Cart", Position = 2, Parent = "GroupService")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ServiceController : BaseController
    {
        private ServiceSV service = new ServiceSV();

        #region View
        [BaseSystem(Name = "Xem", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int serviceCategoryId = 0)
        {
            try
            {
                int total = 0;
                var data = service.GetServices(parameters.Page, parameters.Limit, out total, serviceCategoryId);
                return this.Store(data, total);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadServiceCategories()
        {
            try
            {
                var data = service.GetServiceCategories();
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
        public ActionResult Create(int serviceCategoryId = 0)
        {
            try
            {
                ViewData["ServiceCategoryID"] = serviceCategoryId;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(ServiceItem item)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!service.CheckNameExist(item.Name))
                    {
                        service.Insert(item);
                        Ultilities.ShowMessageRequest(RequestStatus.CreateSuccess);
                        success = true;
                    }
                    else
                    {
                        Ultilities.ShowMessageRequest(RequestStatus.ExistError);
                        success = false;
                    }
                }
                else
                {
                    Ultilities.ShowMessageRequest(RequestStatus.ValidError);
                    success = false;
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }
        #endregion
        #region Update
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int serviceId)
        {
            try
            {
                var data = service.GetService(serviceId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(ServiceItem item)
        {
            var success = false;
            try
            {
                if (!service.CheckNameExist(item.Name, item.ID))
                {
                    service.Update(item);
                    Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                    success = true;
                }
                else
                {
                    Ultilities.ShowMessageRequest(RequestStatus.ExistError);
                    success = false;
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
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
        public ActionResult Delete(int serviceId = 0)
        {
            try
            {
                service.Delete(serviceId);
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
        public ActionResult Detail(int serviceId)
        {
            try
            {
                var data = service.GetService(serviceId);
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
