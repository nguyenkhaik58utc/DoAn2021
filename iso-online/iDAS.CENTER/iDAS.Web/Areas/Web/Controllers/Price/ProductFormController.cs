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
    [BaseSystem(Name = "Hình thức sử dụng", IsActive = true, IsShow = true, Icon = "TagBlue", Position = 1, Parent = GroupPriceProductController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductFormController : BaseController
    {
        ProductFormSV ProductFormSV = new ProductFormSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Thêm, sửa, xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrm(int id = 0, int type = 0, int status = 0)
        {

            Ext.Net.MVC.PartialViewResult partialView = new Ext.Net.MVC.PartialViewResult();
            ProductFormItem obj;
            if (type == (int)Common.FormName.Insert)
            {
                partialView = new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            else if (type == (int)Common.FormName.Edit)
            {
                obj = ProductFormSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
            }
            else if (type == (int)Common.FormName.Detail)
            {
                obj = ProductFormSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return partialView;

        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = ProductFormSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(modules, totalCount);
        }
        public ActionResult LoadDataShow()
        {
            var result = ProductFormSV.GetShow();
            return this.Store(result);
        }
        [ValidateInput(false)]
        public ActionResult Insert(ProductFormItem obj)
        {
            if (insert(obj))
            {
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>("stProductForm").Reload();
            }
            else
            {
                Ultility.CreateNotification(message: Message.InsertError);
            }
            return this.Direct();
        }
        private bool insert(ProductFormItem obj)
        {
            try
            {
                obj.CreateBy = User.ID;
                ProductFormSV.Insert(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [ValidateInput(false)]
        public ActionResult Update(ProductFormItem obj)
        {
            if (update(obj))
            {
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("stProductForm").Reload();
            }
            else
            {
                Ultility.CreateNotification(message: Message.UpdateError);
            }
            return this.Direct();
        }
        private bool update(ProductFormItem obj)
        {
            try
            {
                obj.UpdateBy = User.ID;
                ProductFormSV.Update(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                ProductFormSV.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>("stProductForm").Reload();
            }
            return this.Direct();
        }
    }
}
