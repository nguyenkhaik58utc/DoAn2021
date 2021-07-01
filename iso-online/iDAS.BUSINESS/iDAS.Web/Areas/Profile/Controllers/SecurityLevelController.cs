using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Utilities;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;

namespace iDAS.Web.Areas.Profile.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh mục mức độ bảo mật", Icon = "PageKey", IsActive = true, IsShow = true, Position = 1, Parent = GroupCategoryController.Code)]
    public class SecurityLevelController : BaseController
    {
        ProfileSecuritySV profileSercuritySV = new ProfileSecuritySV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = profileSercuritySV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        public ActionResult GetDataCboSecurity()
        {
            var result = profileSercuritySV.GetAll();
            return this.Store(result);
        }
        //FillColor
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.Common.GetData.RenderColor();
            return this.Store(lst);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm")]
        public ActionResult Insert(string data)
        {
            try
            {
                var objItem = Ext.Net.JSON.Deserialize<ProfileSecurityItem>(data);
                objItem.CreateBy = User.ID;
                if (profileSercuritySV.Insert(objItem))
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                else
                {
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, RequestMessage.Notify, RequestMessage.NameExist);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        public ActionResult Update(string data)
        {
            try
            {
                var objItem = Ext.Net.JSON.Deserialize<ProfileSecurityItem>(data);
                objItem.UpdateBy = User.ID;
                if (profileSercuritySV.Update(objItem))
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                else
                {
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, RequestMessage.Notify, RequestMessage.NameExist);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (profileSercuritySV.Delete(id))
                {
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                else
                {
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, RequestMessage.Notify, RequestMessage.DataUsing);
                }
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult ShowFrm(int id = 0)
        {
            if (id > 0)
            {
                var obj = profileSercuritySV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return null;
        }
    }
}
