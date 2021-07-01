using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh mục loại khách hàng", IsActive = true, IsShow = true, Position = 1, Icon = "TagBlue", Parent = GroupCategoryController.Code)]
    public class CustomerTypeController : BaseController
    {
        private CustomerTypeSV CustomerTypeSV = new CustomerTypeSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = CustomerTypeSV.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            var data = new CustomerTypeItem();
            if (id != 0)
            {
                data = CustomerTypeSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(CustomerTypeItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerTypeSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerTypeSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winCustomerType").Close();
                X.GetCmp<Store>("StoreCustomerType").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    if (CustomerTypeSV.Delete(id))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                        X.GetCmp<Store>("StoreCustomerType").Reload();
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
                    }
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new CustomerTypeItem();
            if (id != 0)
            {
                data = CustomerTypeSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Dùng chung
        /// <summary>
        /// Lấy dữ liệu cho combobox
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadDataActived()
        {
            return this.Store(CustomerTypeSV.GetActived());
        }
        #endregion
    }
}
