using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Base;
using iDAS.DA;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Danh mục nhóm công việc", IsActive = true, Icon = "PageGear", IsShow = true, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TaskCategoryController : BaseController
    {
        //
        // GET: /Task/Category/
        private TaskCategorySV objService = new TaskCategorySV();
        private readonly string storeLevel = "stCategory";
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            List<TaskCategoryItem> lst;
            int total;
            lst = objService.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<TaskCategoryItem>(lst, total));
        }
        public ActionResult GetDataIsUse(StoreRequestParameters parameters)
        {
            List<TaskCategoryItem> lst;
            int total;
            lst = objService.GetAllIsUse(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<TaskCategoryItem>(lst, total));
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, bool inused, string des)
        {
            try
            {
                if (!name.Equals("") && !objService.CheckNameExits(name.Trim()))
                {
                    objService.Insert(name, inused, des, User.ID);
                    X.GetCmp<Store>(storeLevel).Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                    return this.Direct("ErrorExisted");
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
        public ActionResult Update(int id, string name, bool inused, string des)
        {
            try
            {
                objService.Update(id, name, des, inused, User.ID);
                X.GetCmp<Store>(storeLevel).Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                return this.Direct();
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
                objService.Delete(stringId);
                X.GetCmp<Store>(storeLevel).Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
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
