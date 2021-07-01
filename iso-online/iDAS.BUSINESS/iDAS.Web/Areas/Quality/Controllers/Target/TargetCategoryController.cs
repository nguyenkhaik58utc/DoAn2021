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

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Danh mục nhóm mục tiêu", Icon = "PageGear", IsActive = true, IsShow = true, Position = 2, Parent = GroupMenuTargetController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TargetCategoryController : BaseController
    {
        //
        // GET: /Quality/TargetType/
        private QualityTargetCategorySV qualityTargetCategorySV = new QualityTargetCategorySV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = qualityTargetCategorySV.GetAll(filter);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult GetDataActive(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = qualityTargetCategorySV.GetAllActive(filter);
            return this.Store(lst, filter.PageTotal);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            QualityTargetCategoryItem obj = new QualityTargetCategoryItem();
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create" };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id)
        {
            var obj = qualityTargetCategorySV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj };
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                qualityTargetCategorySV.Delete(id);
                X.GetCmp<Store>("stTargetCategory").Reload();
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
            var obj = qualityTargetCategorySV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        public ActionResult ShowFrmListCriteriaCategory()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ListCriteriaCategory" };
        }
        public ActionResult Insert(QualityTargetCategoryItem obj, bool val)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    qualityTargetCategorySV.Insert(obj, User.ID);
                    if (val)
                    {
                        X.GetCmp<Window>("winNewTargetCategory").Close();
                    }
                    X.GetCmp<Store>("stTargetCategory").Reload();
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
        public ActionResult Update(QualityTargetCategoryItem objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    qualityTargetCategorySV.Update(objEdit, User.ID);
                    X.GetCmp<Store>("stTargetCategory").Reload();
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

    }
}
