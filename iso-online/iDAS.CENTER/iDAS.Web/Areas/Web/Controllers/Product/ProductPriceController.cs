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
    [BaseSystem(Name = "Danh sách giá sản phẩm", IsActive = true, IsShow = false, Icon = "Basket", Position = 2, Parent = "GroupProduct")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductPriceController : BaseController
    {
        private ProductPriceSV service = new ProductPriceSV();

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
                var data = service.GetProductPrices(parameters.Page, parameters.Limit, out total, productId);
                return this.Store(data, total);
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
        public ActionResult Create(ProductPriceItem item)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                        service.Insert(item);
                        Ultilities.ShowMessageRequest(RequestStatus.CreateSuccess);
                        success = true;
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
        public ActionResult Update(int productPriceId)
        {
            try
            {
                var data = service.GetProductPrice(productPriceId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(ProductPriceItem item)
        {
            var success = false;
            try
            {
                service.Update(item);
                Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                success = true;
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
        public ActionResult Delete(int productPriceId = 0)
        {
            try
            {
                service.Delete(productPriceId);
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
        public ActionResult Detail(int productPriceId)
        {
            try
            {
                var data = service.GetProductPrice(productPriceId);
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
