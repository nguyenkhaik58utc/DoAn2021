using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;

namespace iDAS.Web.Areas.ISO.Controllers
{
    [BaseSystem(Name = "Quản lý lĩnh vực ngành nghề", Icon = "Compress", IsActive = true, IsShow = true, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ISONaceCodeController : BaseController
    {
        private ISONaceCodeSV naceCodeSV = new ISONaceCodeSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Thêm mới, sửa, xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrm(int id = 0, int type = 0, int status = 0)
        {

            Ext.Net.MVC.PartialViewResult partialView = new Ext.Net.MVC.PartialViewResult();
            ISONaceCodeItem obj;
            if (type == (int)Common.FormName.Insert)
            {
                partialView = new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            else if (type == (int)Common.FormName.Edit)
            {
                obj = naceCodeSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
            }
            else if (type == (int)Common.FormName.Detail)
            {
                obj = naceCodeSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return partialView;

        }
        public ActionResult LoadISONaceCode(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = naceCodeSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(modules, totalCount);
        }
        public ActionResult LoadISONaceCodeActive()
        {
            var modules = naceCodeSV.GetActive();
            return this.Store(modules);
        }
        [ValidateInput(false)]
        public ActionResult InsertISONaceCode(ISONaceCodeItem obj)
        {
            if (insert(obj))
            {
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>("stISONaceCode").Reload();
            }

            return this.Direct();

        }

        [ValidateInput(false)]
        public ActionResult UpdateISONaceCode(ISONaceCodeItem obj)
        {
            if (update(obj))
            {
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("stISONaceCode").Reload();
            }
            return this.Direct();

        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                naceCodeSV.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>("stISONaceCode").Reload();
            }
            return this.Direct();

        }
        private bool insert(ISONaceCodeItem obj)
        {
            try
            {
                naceCodeSV.Insert(obj, User.ID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool update(ISONaceCodeItem obj)
        {
            try
            {
                naceCodeSV.Update(obj, User.ID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
