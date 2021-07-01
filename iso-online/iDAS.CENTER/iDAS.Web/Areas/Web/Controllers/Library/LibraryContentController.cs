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
using iDAS.Utilities;
namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Nội dung thư viện", IsActive = true, IsShow = false, Parent = GroupLibraryController.Code, Icon = "Image")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class LibraryContentController : BaseController
    {
        private LibraryContentSV service = new LibraryContentSV();

        #region View
        [BaseSystem(Name = "Xem", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int libraryId = 0)
        {
            try
            {
                int total = 0;
                var data = service.GetLibraryContents(parameters.Page, parameters.Limit, out total, libraryId);
                return this.Store(data, total);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        #region Create
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Create(int libraryId = 0)
        {
            try
            {
                ViewData["LibraryId"] = libraryId;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(LibraryContentItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (ModelState.IsValid)
                    {
                        if (!service.CheckExist(item))
                        {
                            service.Insert(item);
                            Ultilities.ShowMessageRequest(RequestStatus.CreateSuccess);
                            result = true;
                        }
                        else
                        {
                            Ultilities.ShowMessageRequest(RequestStatus.ExistError);
                        }
                    }
                }
                else
                {
                    Ultilities.ShowMessageRequest(RequestStatus.ValidError);
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(result);
        }
        #endregion
        #region Update
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int LibraryContentId)
        {
            try
            {
                var data = service.GetLibraryContent(LibraryContentId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(LibraryContentItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!service.CheckExist(item))
                    {
                        service.Update(item);
                        Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                        result = true;
                    }
                    else
                    {
                        Ultilities.ShowMessageRequest(RequestStatus.ExistError);
                    }
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(result);
        }
        #endregion
        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int LibraryContentId = 0)
        {
            try
            {
                service.Delete(LibraryContentId);
                Ultilities.ShowMessageRequest(RequestStatus.DeleteSuccess);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.DeleteError);
            }
            return this.Direct();
        }
        #endregion
        #region Detail
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Detail(int libraryContentId)
        {
            try
            {
                var data = service.GetLibraryContent(libraryContentId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion 
    }
}
