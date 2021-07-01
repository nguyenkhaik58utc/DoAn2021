using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Core;
using iDAS.Utilities;

using System.Collections;
using iDAS.Base;
using iDAS.Web.Areas.Systems;

namespace iDAS.Web.Areas.ISO.Controllers
{

    [BaseSystem(Name = "Quản lý thông tin Họp xem xét", IsActive = true, IsShow = true, Position = 4, Icon = "Newspaper")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class MeetingController : BaseController
    {
        IsoMeetingService isoMeetingSV = new IsoMeetingService();
        ISOStandardSV isoV = new ISOStandardSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int isoStandardId = 0)
        {
            int totalCount;
            var modules = isoMeetingSV.GetByISO(parameters.Page, parameters.Limit, out totalCount, isoStandardId);
            return this.Store(modules, totalCount);
        }
        [BaseSystem(Name = "Thêm, sửa, xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmEdit(int id = 0, int type = 0)
        {
            try
            {
                if (id > 0)
                {
                    var obj = isoMeetingSV.GetByID(id);
                    if (type == (int)iDAS.Utilities.Common.FormName.Edit)
                        return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
                    else if (type == (int)iDAS.Utilities.Common.FormName.Detail)
                        return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
                }
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult LoadIso()
        {
            var isoes = isoV.GetISOActived();
            return this.Store(isoes);
        }
        public ActionResult Insert(ISOMeetingItem obj)
        {
            obj.CreateBy = User.ID;
            isoMeetingSV.Insert(obj);
            Ultility.CreateNotification(message: Message.InsertSuccess);
            X.GetCmp<Store>("stIsoMeeting").Reload();
            return this.Direct();
        }
        public ActionResult Update(ISOMeetingItem obj)
        {
            obj.UpdateBy = User.ID;
            isoMeetingSV.Update(obj);
            Ultility.CreateNotification(message: Message.UpdateSuccess);
            X.GetCmp<Store>("stIsoMeeting").Reload();
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    isoMeetingSV.Delete(id);
                    Ultility.CreateNotification(message: Message.DeleteSuccess);
                    X.GetCmp<Store>("stIsoMeeting").Reload();
                }
                catch (Exception ex)
                {
                    Ultility.CreateNotification(message: Message.DeleteDatabaseError);
                }
            }
            return this.Direct();
        }
    }
}
