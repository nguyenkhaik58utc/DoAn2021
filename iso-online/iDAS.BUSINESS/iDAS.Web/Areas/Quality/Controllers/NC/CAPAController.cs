using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "CAPA")]
    public class CAPAController : BaseController
    {
        private QualityCAPASV CAPASV = new QualityCAPASV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult ListView(int id = 0)
        {
            ViewData["id"] = id;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateView(int id = 0)
        {
            var data = CAPASV.GetByRequireId(id);
            if (data.IsEdit)
            {
                return new Ext.Net.MVC.PartialViewResult
                {
                    ViewName = "UpdateView",
                    Model = data,
                };
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult
                {
                    ViewName = "DetailView",
                    Model = data,
                };
            }
        }
        public ActionResult Update(QualityCAPAItem item)
        {
            var success = false;
            try
            {
                if (item.ID != 0)
                {
                    item.UpdateBy = User.ID;
                    CAPASV.Update(item);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    success = true;
                    if (!item.IsEdit)
                    {
                        NotifyController notify = new NotifyController();
                        notify.Notify(this.ModuleCode, "Yêu cầu phê duyệt hành động khắc phục", item.Name, item.Approval.ID, User, Common.CAPARequire, "CAPAID:" + item.ID.ToString());
                    }
                }
                else
                {
                    item.CreateBy = User.ID;
                    var id = CAPASV.Insert(item);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    success = true;
                    if (!item.IsEdit)
                    {
                        NotifyController notify = new NotifyController();
                        notify.Notify(this.ModuleCode, "Yêu cầu phê duyệt hành động khắc phục", item.Name, item.Approval.ID, User, Common.CAPARequire, "CAPAID:" + id);
                    }
                }
                X.GetCmp<Store>("StoreCAPA").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.FormPanel(success);
        }

        public ActionResult ApproveForm(int id = 0)
        {
            var data = CAPASV.GetByRequireId(id);
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Approve",
                Model = data,
            };
        }
        public ActionResult Approve(QualityCAPAItem item)
        {
            var success = false;
            try
            {
                item.UpdateBy = User.ID;
                CAPASV.Approve(item);
                Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                var data = CAPASV.GetByID(item.ID);
                List<int> notifyObj = new List<int>() { data.CreateBy.HasValue?(int)data.CreateBy:0 };
                if (data.UpdateBy != null && data.UpdateBy != data.CreateBy)
                {
                    notifyObj.Add((int)data.UpdateBy);
                }
                NotifyController notify = new NotifyController();
                notify.NotifyUser(this.ModuleCode, "Có một hành động khắc phục đã được phê duyệt", data.Name, notifyObj, User, Common.CAPARequire, "CAPAID:" + item.ID.ToString());
                success = true;
                X.GetCmp<Store>("StoreCAPA").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            return this.FormPanel(success);
        }
    }
}
