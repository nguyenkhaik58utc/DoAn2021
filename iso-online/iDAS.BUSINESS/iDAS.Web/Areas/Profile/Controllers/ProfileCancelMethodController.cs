using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using Ext.Net;
using Ext.Net.MVC;

namespace iDAS.Web.Areas.Profile.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Phương thức hủy hồ sơ", Icon = "PageCancel", IsActive = true, IsShow = true, Position = 3, Parent = GroupCategoryController.Code)]
    public class ProfileCancelMethodController : BaseController
    {
        ProfileCancelMethodService cancelMethodSV = new ProfileCancelMethodService();
        ProfileCancelService cancelSV = new ProfileCancelService();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id = 0)
        {
            var obj = cancelMethodSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                string ck = checkvalidDelete(id);
                if (ck.Trim().Equals(""))
                {
                    cancelMethodSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("stDestructionMethod").Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck, 400);

            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }

            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id = 0)
        {
            var obj = cancelMethodSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = cancelMethodSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        public ActionResult Insert(ProfileCategoryItem obj)
        {
            obj.CreateBy = User.ID;
            string ck = checkValid(obj);
            if (ck.Equals(""))
            {
                cancelMethodSV.Insert(obj);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("stDestructionMethod").Reload();
                X.GetCmp<Window>("winNewCancelMethod").Close();
            }
            else
            {
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck, 400);
            }
            return this.Direct();
        }
        public ActionResult Update(ProfileCategoryItem obj)
        {
            string ck = checkValid(obj, obj.ID);
            if (ck.Equals(""))
            {
                obj.UpdateBy = User.ID;
                cancelMethodSV.Update(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("stDestructionMethod").Reload();
                X.GetCmp<Window>("winEditCancelMethod").Close();
            }
            else
            {
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck, 400);
            }
            return this.Direct();
        }
        private string checkValid(ProfileCategoryItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();
            if (id > 0)
            {
                var docItem = cancelMethodSV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()))
                        ckexit = false;
                    if (docItem.IsActive != objDraft.IsActive && !objDraft.IsActive)
                    {
                        var ck = cancelMethodSV.CheckProfileCancelByMethodID(id);
                        if (!ck)
                            return "Phương thức hủy hồ sơ đang được sử dụng trong Biên bản Hủy hồ sơ (trạng thái chờ hủy) nên không được sửa trạng thái!";
                    }
                }
            }
            if (ckexit)
            {
                var doc = cancelMethodSV.GetByName(objDraft.Name);
                if (doc != null)
                {
                    return "Phương thức hủy Hồ sơ đã tồn tại trong hệ thống! Vui lòng nhập phương thức khác!";
                }
            }
            return "";
        }
        private string checkvalidDelete(int id)
        {
            if (id > 0)
            {
                var lst = cancelSV.GetAllByMethodID(id);
                if (lst != null && lst.Count() > 0)
                {
                    return "Dữ liệu trong bản ghi vừa chọn đang được sử dụng nên không thể xóa! Vui lòng kiểm tra lại!";
                }
            }
            return "";
        }
    }
}
