using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Production.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Mức độ ưu tiên", IsActive = true, Icon = "TagBlue", IsShow = true, Position = 1, Parent = GroupCategoryController.Code)]
    public class ProductionLevelController : BaseController
    {
        private ProductionLevelSV ProductionLevelSV = new ProductionLevelSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.Common.GetData.RenderColor();
            return this.Store(lst);
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionLevelSV.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            var data = new ProductionLevelItem();
            if (id != 0)
            {
                data = ProductionLevelSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(ProductionLevelItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    ProductionLevelSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    ProductionLevelSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCommandLevel").Reload();
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
                    ProductionLevelSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StoreCommandLevel").Reload();
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
            var data = new ProductionLevelItem();
            if (id != 0)
            {
                data = ProductionLevelSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Dùng chung
        public ActionResult LoadDataActived()
        {
            var result = ProductionLevelSV.GetActive();
            return this.Store(result);
        }
        #endregion
    }
}
