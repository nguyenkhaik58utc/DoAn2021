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
    [BaseSystem(Name = "Rủi ro", Icon = "FolderBug", IsActive = true, IsShow = true, Parent = RiskGroupController.Code, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskController : BaseController
    {
        private RiskSV riskSV = new RiskSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int categoryId)
        {
            int totalCount;
            var result = riskSV.GetByCategory(parameters.Page, parameters.Limit, out totalCount, categoryId);
            return this.Store(result, totalCount);
        }
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.GetData.RenderColor();
            return this.Store(lst);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int categoryId = 0, string categoryName = "")
        {
            var data = new RiskItem();
            if (id != 0)
            {
                data = riskSV.GetById(id);
                data.ActionForm = "Edit";
            }
            else
            {
                data.CenterRiskCategoryID = categoryId;
                data.RiskCategoryName = categoryName;
                data.ActionForm = "Add";
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    riskSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
                else
                {
                    riskSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: Message.InsertSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRisk").Reload();
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
                    riskSV.Delete(id);
                    X.GetCmp<Store>("StoreRisk").Reload();
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
            var data = new RiskItem();
            if (id != 0)
            {
                data = riskSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
        
    }
}
