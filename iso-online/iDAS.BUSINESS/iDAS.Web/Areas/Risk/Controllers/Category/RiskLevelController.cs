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

namespace iDAS.Web.Areas.Risk.Controllers
{
    [BaseSystem(Name = "Mức độ rủi ro", Icon = "TagBlue", IsActive = true, IsShow = true, Parent = RiskCategoryController.Code, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskLevelController : BaseController
    {
        private RiskLevelSV RiskLevelSV = new RiskLevelSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = RiskLevelSV.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult LoadRiskMethod()
        {
            var result = RiskLevelSV.GetAllRiskMethod();
            return this.Store(result, 1);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            var data = new RiskLevelItem();
            if (id != 0)
            {
                data = RiskLevelSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskLevelItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    if (RiskLevelSV.Update(data, User.ID))
                    { 
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess); 
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify,message: RequestMessage.NameExist);
                    }
                }
                else
                {
                    if (RiskLevelSV.Insert(data, User.ID))
                    {
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.NameExist);
                    }
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRiskLevel").Reload();
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
                    RiskLevelSV.Delete(id);
                    X.GetCmp<Store>("StoreRiskLevel").Reload();
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
            var data = new RiskLevelItem();
            if (id != 0)
            {
                data = RiskLevelSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

    }
}
