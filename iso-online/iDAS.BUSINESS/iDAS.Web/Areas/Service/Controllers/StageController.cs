using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Service.Controllers
{
    [BaseSystem(Name = "Công đoạn cung cấp", Icon = "RubyAdd", IsActive = true, IsShow = true, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class StageController : BaseController
    {
        //
        // GET: /Service/Stage/
        private ServiceStageSV stageService = new ServiceStageSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int serviceId)
        {
            List<ServiceStageItem> lst;
            int total;
            lst = stageService.GetAllByServiceID(parameters.Page, parameters.Limit, out total, serviceId);
            return this.Store(new Paging<ServiceStageItem>(lst, total));
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd(int serviceId)
        {
            ServiceStageItem obj = new ServiceStageItem();
            obj.ServiceID = serviceId;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create", Model = obj };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id)
        {
            var obj = stageService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj };
        }
        public ActionResult ShowFrmListCriteriaCategory()
        { 
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ListCriteriaCategory" };
        }
        public ActionResult Insert(ServiceStageItem obj, bool val)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    stageService.Insert(obj, User.ID);
                    if (val)
                    {
                        X.GetCmp<Window>("winNewStage").Close();
                    }
                    X.GetCmp<Store>("stStage").Reload();
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
        public ActionResult Update(ServiceStageItem objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    stageService.Update(objEdit, User.ID);
                    X.GetCmp<Store>("stStage").Reload();
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
                stageService.Delete(id);
                X.GetCmp<Store>("stStage").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id)
        {
            var obj = stageService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
    }
}
