using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Thư viện hình", IsActive = true, IsShow = true, Parent = GroupLibraryController.Code, Icon = "CameraEdit")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class LibraryVideoController : BaseController
    {
        private LibrarySV LibrarySV = new LibrarySV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var services = LibrarySV.GetVideo(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(services, totalCount);
        }

        #region Thêm
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            catch
            {
                return this.Direct();
            }
        }
        [HttpPost]
        public ActionResult Insert(LibraryItem addData)
        {
            try
            {
                LibrarySV.Insert(addData, User.ID);
                Ultility.CreateNotification(message: Message.InsertSuccess);
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: Message.InsertError, error: true);
                return this.Direct();
            }
        }
        #endregion
        #region Sửa
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id)
        {
            try
            {
                var data = LibrarySV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
            }
            catch
            {
                return this.Direct();
            }
        }
        [HttpPost]
        public ActionResult Update(LibraryItem addData)
        {
            try
            {
                LibrarySV.Update(addData, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError);
                return this.Direct();
            }
        }
        #endregion
        #region Xóa
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                LibrarySV.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>("StoreLibraryVideo").Reload();
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: Message.DeleteError);
                return this.Direct();
            }
        }
        #endregion
        #region Xem
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Detail(int id)
        {
            try
            {
                var data = LibrarySV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
            }
            catch
            {
                return this.Direct();
            }
        }
        #endregion

     

    }
}
