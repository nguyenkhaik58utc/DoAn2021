using iDAS.Core;
using iDAS.DA;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using System.Collections;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Danh mục sản phẩm", IsActive = true, IsShow = true, Icon = "BoxPicture", Position =1, Parent = "GroupProduct")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductCategoryController : BaseController
    {
        private ProductCategorySV service = new ProductCategorySV();

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
                var data = service.GetProductCategories(parameters.Page, parameters.Limit, out total);
                return this.Store(data, total);
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
        public ActionResult Create(ProductCategoryItem productCategory)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!service.CheckNameExist(productCategory.Name))
                    {
                        service.Insert(productCategory);
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
        public ActionResult Update(int productCategoryId)
        {
            try
            {
                var data = service.GetProductCategory(productCategoryId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(ProductCategoryItem item)
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
        public ActionResult Delete(int productCategoryId = 0)
        {
            try
            {
                service.Delete(productCategoryId);
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
        public ActionResult Detail(int productCategoryId)
        {
            try
            {
                var data = service.GetProductCategory(productCategoryId);
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
