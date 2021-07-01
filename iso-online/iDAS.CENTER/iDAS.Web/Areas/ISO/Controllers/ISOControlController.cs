using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.ISO.Controllers
{
    [BaseSystem(Name = "Quản lý kiểm soát kỹ thuật", Icon = "AwardStarBronze2", IsActive = true, IsShow = true, Position = 5)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ISOControlController : BaseController
    {
        private ISOControlSV isoControlSV = new ISOControlSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int isoStandardId)
        {
            int totalCount;
            var result = isoControlSV.GetByISO(parameters.Page, parameters.Limit, out totalCount, isoStandardId);
            return this.Store(result, totalCount);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int isoStandardId = 0)
        {
            var data = new ISOControlItem();
            if (id != 0)
            {
                data = isoControlSV.GetById(id);
                data.ActionForm = "Edit";
            }
            else
            {
                data.ISOStandardID = isoStandardId;
                data.ActionForm = "Add";
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(ISOControlItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    isoControlSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
                else
                {
                    isoControlSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: Message.InsertSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreISOControl").Reload();
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
                    isoControlSV.Delete(id);
                    X.GetCmp<Store>("StoreISOControl").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: Message.Notify, message: Message.DeleteError, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        #endregion
        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new ISOControlItem();
            if (id != 0)
            {
                data = isoControlSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
