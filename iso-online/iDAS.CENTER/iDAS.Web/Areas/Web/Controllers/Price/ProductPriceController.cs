using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Gói sản phẩm", IsActive = true, IsShow = true, Position = 2, Icon = "PackageStart", Parent = GroupPriceProductController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductPriceController : BaseController
    {
        private ProductPakageSV productSV = new ProductPakageSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadPrice(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = productSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(modules, totalCount);
        }
        public ActionResult LoadSystem()
        {
            var systems = productSV.GetProductSystem();
            return this.Store(systems);
        }
        public ActionResult LoadIso()
        {
            var isos = new ISOStandardSV().GetISOActived();
            return this.Store(isos);
        }
        public ActionResult FormAdd()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create" };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Thêm mới thông tin giá sản phẩm", IsActive = true, IsShow = true)]
        [ValidateInput(false)]
        public ActionResult Insert(ProductPakageItem objNew, bool val, string imgService)
        {
            try
            {
                objNew.CreateBy = User.ID;
                productSV.Insert(objNew);

                if (val == true)
                {
                    X.GetCmp<Window>("winCreateProductPrice").Close();
                }
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>("stProductPrice").Reload();
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        public ActionResult ShowUpdate(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = productSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult ShowDetail(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = productSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Sửa thông tin bảng giá sản phẩm", IsActive = true, IsShow = true)]
        public ActionResult Update(ProductPakageItem objEdit, string imgService)
        {
            try
            {

                objEdit.UpdateBy = User.ID;
                productSV.Update(objEdit);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("stProductPrice").Reload();
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }

        [BaseSystem(Name = "Xóa thông tin giá sản phẩm", IsActive = true, IsShow = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {

                var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in ids)
                {
                    productSV.Delete((int)item);
                }
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>("stProductPrice").Reload();
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã có lỗi xảy ra trong quá trình xóa giá sản phẩm!"
                });
                return this.Direct("Error");
            }
        }
    }
}
