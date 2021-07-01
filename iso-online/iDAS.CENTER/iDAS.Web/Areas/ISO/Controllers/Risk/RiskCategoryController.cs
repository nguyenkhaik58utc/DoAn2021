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
    [BaseSystem(Name = "Nhóm rủi ro", Icon = "TagBlue", IsActive = true, IsShow = true, Parent = RiskGroupController.Code, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskCategoryController : BaseController
    {
        private RiskCategorySV RiskCategorySV = new RiskCategorySV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {       
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int isoStandardId)
        {
            int totalCount;
            var result = RiskCategorySV.GetByISO(parameters.Page, parameters.Limit, out totalCount, isoStandardId);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// dùng chung: Lấy danh sách nhóm rủi ro
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadAll(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = RiskCategorySV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int isoStandardId = 0)
        {
            var data = new RiskCategoryItem();
            if (id != 0)
            {
                data = RiskCategorySV.GetById(id);
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
        public ActionResult Update(RiskCategoryItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    RiskCategorySV.Update(data, User.ID);
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
                else
                {
                    RiskCategorySV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: Message.InsertSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRiskCategory").Reload();
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
                    RiskCategorySV.Delete(id);
                    X.GetCmp<Store>("StoreRiskCategory").Reload();
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
            var data = new RiskCategoryItem();
            if (id != 0)
            {
                data = RiskCategorySV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

    }
}
