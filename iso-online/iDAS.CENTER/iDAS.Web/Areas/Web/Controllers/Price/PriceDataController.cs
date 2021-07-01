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

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Giá dung lượng", IsActive = true, IsShow = true, Position = 3, Icon = "DatabaseYellow", Parent = "GroupPriceProductController.Code")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class PriceDataController : BaseController
    {
        //
        // GET: /Web/FAQCategories/
        PriceDataSizeSV PriceDataSizeSV = new PriceDataSizeSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        // [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Thêm, sửa, xem chi tiết", IsActive = true, IsShow = false)]
        // [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrm(int id = 0, int type = 0, int status = 0)
        {

            Ext.Net.MVC.PartialViewResult partialView = new Ext.Net.MVC.PartialViewResult();
            PriceDataSizeItem obj;
            if (type == (int)Common.FormName.Insert)//form them moi
            {
                partialView = new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            else if (type == (int)Common.FormName.Edit)
            {
                obj = PriceDataSizeSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
            }
            else if (type == (int)Common.FormName.Detail)
            {
                obj = PriceDataSizeSV.GetById(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return partialView;
        }
        public ActionResult LoadPriceDataSize(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = PriceDataSizeSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }
        //InsertLicense
        public ActionResult InsertPriceDataSize(PriceDataSizeItem obj)
        {
            obj.CreateBy = User.ID;
            if (insert(obj))
            {
                X.GetCmp<FormPanel>("frmNewPriceDataSize").Reset();
                X.GetCmp<Window>("winNewPriceDataSize").Close();
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>("stPriceDataSize").Reload();
            }
            return this.Direct();
        }
        private bool insert(PriceDataSizeItem obj)
        {
            try
            {
                obj.UpdateBy = User.ID;
                obj.UpdateAt = DateTime.Now;
                PriceDataSizeSV.Insert(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //
        public ActionResult UpdatePriceDataSize(PriceDataSizeItem obj)
        {
            if (update(obj))
            {
                X.GetCmp<FormPanel>("frmEditPriceDataSize").Reset();
                X.GetCmp<Window>("winEditPriceDataSize").Close();
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("stPriceDataSize").Reload();
            }
            return this.Direct();
        }
        private bool update(PriceDataSizeItem obj)
        {
            try
            {
                obj.UpdateBy = User.ID;
                obj.UpdateAt = DateTime.Now;
                PriceDataSizeSV.Update(obj, obj.ID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        // [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                PriceDataSizeSV.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
                X.GetCmp<Store>("stPriceDataSize").Reload();
            }
            return this.Direct();
        }
    }
}
