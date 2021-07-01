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
namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Danh sách tệp đính kèm", IsActive = true, IsShow = false, Icon = "Cart", Position = 2, Parent = "GroupService")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ServiceProgramFileController : BaseController
    {
        private ServiceProgramFileSV service = new ServiceProgramFileSV();

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
        public ActionResult LoadData(StoreRequestParameters parameters, int serviceProgramId = 0)
        {
            try
            {
                int total = 0;
                var data = service.GetServiceProgramFiles(parameters.Page, parameters.Limit, out total, serviceProgramId);
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
        public ActionResult Create(int serviceProgramId = 0)
        {
            try
            {
                ViewData["ServiceProgramID"] = serviceProgramId;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(ServiceProgramFileItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                        service.Insert(item);
                        Ultilities.ShowMessageRequest(RequestStatus.CreateSuccess);
                        result = true;
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
        public ActionResult Update(int serviceProgramFileId)
        {
            try
            {
                var data = service.GetServiceProgramFile(serviceProgramFileId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(ServiceProgramFileItem item)
        {
            var result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    service.Update(item);
                    Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                    result = true;
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
        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int serviceProgramFileId = 0)
        {
            try
            {
                service.Delete(serviceProgramFileId);
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
        public ActionResult Detail(int serviceProgramFileId)
        {
            try
            {
                var data = service.GetServiceProgramFile(serviceProgramFileId);
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
