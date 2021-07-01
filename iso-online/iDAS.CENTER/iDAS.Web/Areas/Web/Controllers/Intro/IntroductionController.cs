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

    [BaseSystem(Name = "Giới thiệu công ty", IsActive = true, IsShow = true, Position = 1, Icon = "PageWhiteText")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class IntroductionController : BaseController
    {
        WebIntroSV WebIntroSV = new WebIntroSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Thêm, sửa, xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrm(int id = 0, int type = 0, int status = 0)
        {

            Ext.Net.MVC.PartialViewResult partialView = new Ext.Net.MVC.PartialViewResult();
            IntroItem obj;
            if (type == (int)Common.FormName.Insert)//form them moi
            {
                partialView = new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            else if (type == (int)Common.FormName.Edit)
            {
                obj = WebIntroSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
            }
            else if (type == (int)Common.FormName.Detail)
            {
                obj = WebIntroSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return partialView;
        }
        public ActionResult LoadIntro(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = WebIntroSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }
        [ValidateInput(false)]
        public ActionResult InsertCate(IntroItem obj)
        {
            bool Success = false;
            if (insert(obj))
            {
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>("stCateSV").Reload();
                Success = true;
            }
            return this.Direct(Success);
        }
        private bool insert(IntroItem obj)
        {
            try
            {
                obj.CreateBy = User.ID;
                obj.CreateAt = DateTime.Now;
                WebIntroSV.Insert(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [ValidateInput(false)]
        public ActionResult UpdateCate(IntroItem obj)
        {
            bool Success = false;
            if (update(obj))
            {
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("stCateSV").Reload();
                Success = true;
            }
            return this.Direct(Success);
        }
        private bool update(IntroItem obj)
        {
            try
            {
                obj.UpdateBy = User.ID;
                obj.UpdateAt = DateTime.Now;
                WebIntroSV.Update(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                WebIntroSV.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>("stCateSV").Reload();
            }
            return this.Direct();
        }
    }
}
