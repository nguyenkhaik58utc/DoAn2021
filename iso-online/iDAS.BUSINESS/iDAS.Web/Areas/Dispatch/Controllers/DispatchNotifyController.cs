using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [BaseSystem(Name = "Thông báo", IsActive = true, Icon = "Bell", IsShow = true, Position = 7)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DispatchNotifyController : BaseController
    {
        //
        // GET: /Dispatch/DispatchNotify/
        private BusinessNotifySV notifySV = new BusinessNotifySV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var notify = notifySV.GetByModule(filter, User.EmployeeID, this.ModuleCode);
            return this.Store(notify, filter.PageTotal);
        }
        public ActionResult ShowFrmRead(int id)
        {
            notifySV.UpdateRead(id, User.ID);
            var obj = notifySV.GetByID(id, SystemConfig.Config.SystemCode);
            X.GetCmp<Store>("stNotifyStores").Reload();
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Read", Model = obj };
        }
        public ActionResult CheckRead(string stringId = "")
        {
            try
            {
                if (stringId != "")
                {
                    notifySV.ReadMany(stringId, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stNotifyStores").Reload();
            }
            return this.Direct();
        }
        public ActionResult Delete(string stringId = "")
        {
            try
            {
                if (stringId != "")
                {
                    notifySV.Delete(stringId);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stNotifyStores").Reload();
            }
            return this.Direct();
        }
        public ActionResult UnRead(string stringId = "")
        {
            try
            {
                if (stringId != "")
                {
                    notifySV.UnRead(stringId, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stNotifyStores").Reload();
            }
            return this.Direct();
        }
    }
}
