using iDAS.Core;
using iDAS.DA;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Base;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Danh mục lịch", IsActive = true, Icon = "PageGear", IsShow = true, Position = 1, Parent = CalendarGroupController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TaskCalendarEventController : BaseController
    {
        //
        // GET: /Task/TaskCalendarEvent/
        private TaskCalendarEventSV taskLevelSV = new TaskCalendarEventSV();
        private readonly string storeLevel = "stTaskCalendarEvent";
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadStore(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = taskLevelSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst);
        }
        public ActionResult LoadStoreIsUse(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = taskLevelSV.GetAllIsUse(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst);
        }
        public ActionResult getListData(StoreRequestParameters parameters)
        {
            List<TaskCalendarEventsItem> lst;
            int total;
            lst = taskLevelSV.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<TaskCalendarEventsItem>(lst, total));
        }
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.Common.GetData.RenderColor();
            return this.Store(lst);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, string color, bool isworking ,bool inused, string des)
        {
            try
            {
                if (!name.Equals("") && !taskLevelSV.CheckNameExits(name.Trim()))
                {
                    taskLevelSV.Insert(name, color, isworking, inused, des, User.ID);
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
        public ActionResult Update(int id, string name, string color, bool isworking, bool inused, string des)
        {
            try
            {
                if (!name.Equals("") && !taskLevelSV.CheckNameEditExits(id, name))
                {
                    taskLevelSV.Update(id, name, des, color,isworking, inused, User.ID);
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
                taskLevelSV.Delete(stringId);
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
