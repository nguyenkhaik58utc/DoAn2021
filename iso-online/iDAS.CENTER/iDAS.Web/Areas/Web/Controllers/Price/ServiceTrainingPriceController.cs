using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Đào tạo", IsActive = true, IsShow = true, Icon = "ApplicationEdit", Position = 1, Parent = GroupPriceServiceController.Code)]
    public class ServiceTrainingPriceController : BaseController
    {
        private ServiceTrainingPriceSV ServicePriceSV = new ServiceTrainingPriceSV();
        private ISOStandardSV isoSV = new ISOStandardSV();
        [BaseSystem(Name = "Quản lý bảng giá dịch vụ", IsActive = true, IsShow = true, Icon = "Basket")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = ServicePriceSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(modules, totalCount);
        }
        public ActionResult LoadIso()
        {
            var modules = isoSV.GetISOActived();
            return this.Store(modules);
        }
        public ActionResult LoadServiceForm()
        {
            var result = ServicePriceSV.GetServiceForm();
            return this.Store(result);
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
        /// <summary>
        /// Hàm insert dữ liệu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Thêm mới thông tin giá dịch vụ", IsActive = true, IsShow = true)]
        [ValidateInput(false)]
        public ActionResult Insert(ServiceTrainingPriceItem objNew, bool val, string imgService)
        {
            try
            {

                objNew.CreateBy = User.ID;
                ServicePriceSV.Insert(objNew);

                if (val == true)
                {
                    X.GetCmp<Window>("winNewServicePrice").Close();
                }
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>("stMnServicePrice").Reload();
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
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = ServicePriceSV.GetById(id) };
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
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = ServicePriceSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        /// <summary>
        /// Hàm update dữ liệu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Sửa thông tin bảng giá dịch vụ", IsActive = true, IsShow = true)]
        public ActionResult Update(ServiceTrainingPriceItem objEdit, string imgService)
        {
            try
            {

                objEdit.UpdateBy = User.ID;
                ServicePriceSV.Update(objEdit);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("stMnServicePrice").Reload();
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }

        [BaseSystem(Name = "Xóa thông tin giá dịch vụ", IsActive = true, IsShow = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {

                var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in ids)
                {
                    ServicePriceSV.Delete((int)item);
                }
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>("stMnServicePrice").Reload();
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã có lỗi xảy ra trong quá trình xóa giá dịch vụ!"
                });
                return this.Direct("Error");
            }
        }

    }
}
