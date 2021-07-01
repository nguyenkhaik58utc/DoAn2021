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
namespace iDAS.Web.Areas.Production.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Yêu cầu", IsActive = true, IsShow = true, Position = 2, Icon = "CartEdit")]
    public class RequiredController : BaseController
    {
        private ProductionRequirementSV ProductionRequirementSV = new ProductionRequirementSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionRequirementSV.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            var data = new ProductionRequirementItem();
            if (id != 0)
            {
                data = ProductionRequirementSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(ProductionRequirementItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    ProductionRequirementSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    ProductionRequirementSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequired").Reload();
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
                    ProductionRequirementSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StoreRequired").Reload();
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
            var data = new ProductionRequirementItem();
            if (id != 0)
            {
                data = ProductionRequirementSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Tạm dừng sản xuất
        public ActionResult Pause(int id)
        {
            try
            {
                ProductionRequirementSV.Pause(id);
                X.GetCmp<Store>("StoreRequired").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Tiếp tục sản xuất
        public ActionResult Play(int id)
        {
            try
            {
                ProductionRequirementSV.Pause(id, false);
                X.GetCmp<Store>("StoreRequired").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Kết thúc sản xuất
        [HttpGet]
        public ActionResult Finish(int id)
        {
            var data = new ProductionRequirementItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Finish", Model = data };
        }
        [HttpPost]
        public ActionResult Finish(ProductionRequirementItem data)
        {
            ProductionRequirementSV.Finish(data.ID, data.FinishTime);
            X.GetCmp<Store>("StoreRequired").Reload();
            return this.Direct();
        }
        #endregion

        #region Dùng chung
        public ActionResult LoadCombobox()
        {
            var result = ProductionRequirementSV.GetAll();
            return this.Store(result);
        }
        #endregion
    }
}

