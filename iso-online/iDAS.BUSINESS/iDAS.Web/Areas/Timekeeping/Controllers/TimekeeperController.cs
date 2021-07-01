using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Timekeeping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Timekeeping.Controllers
{
    [BaseSystem(Name = "Danh sách máy chấm công", Icon = "Clock", IsActive = true, IsShow = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TimekeeperController : BaseController
    {
        private TimekeeperAPI api = new TimekeeperAPI();
        // GET: Timekeeping/Timekeeper
        public ActionResult Index()
        {
            return View();
        }

        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create" };
        }

        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id)
        {
            var obj = api.GetByID(id).Result;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj };
        }

        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id)
        {
            var obj = api.GetByID(id).Result;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }

        public ActionResult GetData(StoreRequestParameters parameters)
        {
            var resp = api.GetData(parameters.Page, parameters.Limit);
            return this.Store(resp.Result);
        }

        public ActionResult Insert(TimekeeperDTO obj, bool val)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Insert(obj);
                    if (val)
                    {
                        X.GetCmp<Window>("winNewTimekeeper").Close();
                    }
                    X.GetCmp<Store>("stTimekeeper").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }

        public ActionResult Update(TimekeeperDTO objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Update(objEdit);
                    X.GetCmp<Store>("stTimekeeper").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }

        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                api.Delete(id);
                X.GetCmp<Store>("stTimekeeper").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }
        }

        [BaseSystem(Name = "Lấy dữ liệu", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult InputDataLog(int id,  string ip, int post)
        {
            try
            {
                TimekeeperDTO obj = new TimekeeperDTO();
                obj.ID = id;
                obj.IP = ip;
                obj.Post = post;

                var result = api.InputDataLog(obj).Result;
                Ultility.CreateNotification(message: result);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
    }
}