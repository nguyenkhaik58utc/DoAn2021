using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Dispatch.Controllers
{

    [BaseSystem(Name = "Danh mục mức độ khẩn ", Icon = "PageGear", IsActive = true, IsShow = true, Position = 4, Parent = DispatchCommonMenuController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DispatchUrgencyController : BaseController
    {
        private DispatchUrgencySV dispatchUrgencySV = new DispatchUrgencySV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = dispatchUrgencySV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, string color, bool isused, string des)
        {
            try
            {
                var objItem = convertToTypeItem(name, color, isused, des);
                string ck = checkValid(objItem);
                if (ck.Equals(""))
                {
                    objItem.CreateBy = User.ID;
                    dispatchUrgencySV.Insert(objItem);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int id, string name, string color, bool isused, string des)
        {
            try
            {
                var objItem = convertToTypeItem(name, color, isused, des, id);
                string ck = checkValid(objItem, id);
                if (ck.Equals(""))
                {
                    objItem.UpdateBy = User.ID;
                    dispatchUrgencySV.Update(objItem);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                string ck = checkValidDelete(id);
                if (ck.Equals(""))
                {
                    dispatchUrgencySV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("stSecurityLevel").Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrm(int id = 0)
        {
            if (id > 0)
            {
                var obj = dispatchUrgencySV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return null;
        }
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.Common.GetData.RenderColor();
            return this.Store(lst);
        }
        private string checkValid(DispatchSecurityItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();
            if (id > 0)
            {
                var docItem = dispatchUrgencySV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = dispatchUrgencySV.GetByName(objDraft.Name);
                if (doc != null)
                {
                    return "Tên Mức độ khẩn đã tồn tại trên hệ thống. Vui lòng nhập lại Tên mức độ khẩn!";
                }
            }
            return "";
        }
        private string checkValidDelete(int id)
        {
            if (id > 0)
            {
                var docItem = dispatchUrgencySV.CheckNotIsUse(id);
                if (!docItem)
                {
                    return "Mức độ khẩn đã được sử dụng trong bảng khác trên hệ thống nên không được phép Xóa!";
                }
            }
            return "";
        }
        private DispatchSecurityItem convertToTypeItem(string name, string color, bool inused, string des, int id = 0)
        {
            var objItem = new DispatchSecurityItem
            {
                Name = name.Trim(),
                Color = color,
                Note = des,
                IsActive = inused,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
            };
            if (id > 0) objItem.ID = id;
            return objItem;
        }
    }
}
