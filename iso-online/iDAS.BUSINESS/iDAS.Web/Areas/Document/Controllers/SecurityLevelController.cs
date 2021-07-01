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


namespace iDAS.Web.Areas.Document.Controllers
{
    [BaseSystem(Name = "Danh mục mức độ bảo mật", Icon = "PageKey", IsActive = true, IsShow = true, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class SecurityLevelController : BaseController
    {
        DocumentSecuritySV docSercuritySV = new DocumentSecuritySV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = docSercuritySV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm ")]
        public ActionResult Insert(string name, string color, bool isused, string des)
        {
            try
            {
                var objItem = convertToTypeItem(name, color, isused, des);
                string ck = checkValid(objItem);
                if (ck.Equals(""))
                {
                    objItem.CreateBy = User.ID;
                    docSercuritySV.Insert(objItem);
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
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa ")]
        public ActionResult Update(int id, string name, string color, bool isused, string des)
        {
            try
            {
                var objItem = convertToTypeItem(name, color, isused, des, id);
                string ck = checkValid(objItem, id);
                if (ck.Equals(""))
                {
                    objItem.UpdateBy = User.ID;
                    docSercuritySV.Update(objItem);
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
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa ")]
        public ActionResult Delete(int id)
        {
            try
            {
                string ck = checkValidDelete(id);
                if (ck.Equals(""))
                {
                    docSercuritySV.Delete(id);
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
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết ")]
        public ActionResult ShowFrm(int id = 0)
        {
            if (id > 0)
            {
                var obj = docSercuritySV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return null;
        }
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.Common.GetData.RenderColor();
            return this.Store(lst);
        }
        private string checkValid(DocumentSecurityItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();
            if (id > 0)
            {
                var docItem = docSercuritySV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = docSercuritySV.GetByName(objDraft.Name);
                if (doc != null)
                {
                    return "Tên Mức độ bảo mật đã tồn tại trên hệ thống. Vui lòng nhập lại Tên mức độ bảo mật!";
                }
            }
            return "";
        }
        private string checkValidDelete(int id)
        {
            if (id > 0)
            {
                var docItem = docSercuritySV.CheckNotIsUse(id);
                if (!docItem)
                {
                    return "Mức độ bảo mật đã được sử dụng trong bảng khác trên hệ thống nên không được phép Xóa!";
                }
            }
            return "";
        }
        private DocumentSecurityItem convertToTypeItem(string name, string color, bool inused, string des, int id = 0)
        {
            var objItem = new DocumentSecurityItem
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
