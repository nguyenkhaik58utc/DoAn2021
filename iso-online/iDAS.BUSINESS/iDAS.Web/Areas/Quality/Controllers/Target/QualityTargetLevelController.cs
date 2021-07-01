using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Danh mục mức độ ưu tiên mục tiêu", Icon = "PageGear", IsActive = true, IsShow = false, Position = 1, Parent = GroupMenuTargetController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class QualityTargetLevelController : BaseController
    {
        //
        // GET: /Quality/QualityTargetLevel/

        private QualityTargetLevelSV qualityTargetLevelSV = new QualityTargetLevelSV();
        private readonly string storeLevel = "stQualityTargetLevelList";
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Index" };
        }
        public ActionResult LoadStore(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = qualityTargetLevelSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst);
        }
        public ActionResult LoadStoreIsUse(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = qualityTargetLevelSV.GetAllIsUse(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst);
        }
        public ActionResult getListData(StoreRequestParameters parameters)
        {
            List<QualityTargetLevelItem> lst;
            int total;
            lst = qualityTargetLevelSV.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<QualityTargetLevelItem>(lst, total));
        }
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.Common.GetData.RenderColor();
            return this.Store(lst);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, string color, bool inused, string des)
        {
            try
            {
                if (!name.Equals("") && !qualityTargetLevelSV.CheckNameExits(name.Trim()))
                {
                    qualityTargetLevelSV.Insert(name, color, inused, des, User.ID);
                    X.GetCmp<Store>(storeLevel).Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: RequestMessage.NameExist, icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct("ErrorExisted");
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int id, string name, string color, bool inused, string des)
        {
            try
            {
                if (!name.Equals("") && !qualityTargetLevelSV.CheckNameEditExits(id, name))
                {
                    qualityTargetLevelSV.Update(id, name, des, color, inused, User.ID);
                    X.GetCmp<Store>(storeLevel).Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
                else
                {
                    return this.Direct("Error");
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                qualityTargetLevelSV.Delete(stringId);               
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>(storeLevel).Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }

    }
}
