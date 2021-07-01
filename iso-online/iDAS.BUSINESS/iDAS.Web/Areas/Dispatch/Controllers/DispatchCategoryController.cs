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
    [BaseSystem(Name = "Danh mục nhóm công văn ", IsActive = true, IsShow = true, Position = 1, Parent = DispatchCommonMenuController.Code, Icon = "PageLink")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DispatchCategoryController : BaseController
    {
        DispatchCategorySV dispatchCateSV = new DispatchCategorySV();
        string store = "stDispatchCate";
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = dispatchCateSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, string des, bool isUse)
        {
            try
            {
                var objItem = convertToTypeItem(isUse, name, des);
                string ck = checkValid(objItem);
                if (ck.Equals(""))
                {
                    objItem.CreateBy = User.ID;
                    dispatchCateSV.Insert(objItem);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>(store).Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);
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
        public ActionResult Update(int id, string name, string des, bool isUse)
        {
            try
            {
                var objItem = convertToTypeItem(isUse, name, des, id);
                string ck = checkValid(objItem, id);
                if (ck.Equals(""))
                {
                    objItem.UpdateBy = User.ID;
                    dispatchCateSV.Update(objItem);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>(store).Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);
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
        public ActionResult Delete(int id)
        {
            try
            {
                string ck = checkValidDelete(id);
                if (ck.Equals(""))
                {
                    dispatchCateSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>(store).Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);

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
                var obj = dispatchCateSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return null;
        }
        private string checkValid(DispatchCategoryItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();
            if (id > 0)
            {
                var docItem = dispatchCateSV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()) && !docItem.IsDelete)
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = dispatchCateSV.GetByName(objDraft.Name);
                if (doc != null)
                {
                    return "Tên nhóm công văn đã tồn tại trên hệ thống. Vui lòng nhập lại Tên nhóm công văn!";
                }
            }
            return "";
        }
        private string checkValidDelete(int id)
        {
            if (id > 0)
            {
                var ck = dispatchCateSV.CheckNotIsUse(id);
                if (!ck)
                {
                    return "Nhóm công văn đã được sử dụng trong bảng khác trên hệ thống nên không được phép Xóa!";
                }
            }
            return "";
        }
        private DispatchCategoryItem convertToTypeItem(bool isUse, string name, string des, int id = 0)
        {
            var objItem = new DispatchCategoryItem
            {
                Name = name.Trim(),
                Note = des,
                IsUse = isUse,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
            };
            if (id > 0) objItem.ID = id;
            return objItem;
        }
    }
}
