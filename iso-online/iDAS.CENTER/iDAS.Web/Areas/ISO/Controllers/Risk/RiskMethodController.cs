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
    [BaseSystem(Name = "Ứng phó rủi ro", Icon = "LightningAdd", IsActive = true, IsShow = true, Parent = RiskGroupController.Code, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskMethodController : BaseController
    {
        private RiskMethodSV riskMethodSV = new RiskMethodSV();
        [BaseSystem(Name = "Xem danh sách")] 
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {          
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = riskMethodSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
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
        public ActionResult Update(int id = 0)
        {
            var data = new RiskMethodItem();
            if (id != 0)
            {
                data = riskMethodSV.GetById(id);
                data.ActionForm = "Edit";
            }
            else
            {
                data.ActionForm = "Add";
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskMethodItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    riskMethodSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
                else
                {
                    riskMethodSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: Message.InsertSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRiskMethod").Reload();
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
                    riskMethodSV.Delete(id);
                    X.GetCmp<Store>("StoreRiskMethod").Reload();
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
            var data = new RiskMethodItem();
            if (id != 0)
            {
                data = riskMethodSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

    }
}
