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
    [BaseSystem(Name = "Danh sách module sản phẩm", IsActive = true, IsShow = false, Icon = "Basket", Position = 2, Parent = "GroupProduct")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductModuleController : BaseController
    {
        private ProductModuleSV service = new ProductModuleSV();

        #region View
        [BaseSystem(Name = "Xem", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
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
        public ActionResult LoadData(StoreRequestParameters parameters, int productId = 0)
        {
            try
            {
                int total = 0;
                var data = service.GetProductModules(parameters.Page, parameters.Limit, out total, productId);
                return this.Store(data, total);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadModules(int productId = 0, int moduleId = 0)
        {
            try
            {
                var data = service.GetModules(productId, moduleId);
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
        public ActionResult Create(int productId = 0)
        {
            try
            {
                ViewData["ProductId"] = productId;
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
        public ActionResult Create(ProductModuleItem item)
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
        public ActionResult Update(int productModuleId)
        {
            try
            {
                var data = service.GetProductModule(productModuleId);
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
        public ActionResult Update(ProductModuleItem item)
        {
            var result = false;
            try
            {
                service.Update(item);
                Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                result = true;
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
        public ActionResult Delete(int productModuleId = 0)
        {
            try
            {
                service.Delete(productModuleId);
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
        public ActionResult Detail(int productModuleId)
        {
            try
            {
                var data = service.GetProductModule(productModuleId);
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
