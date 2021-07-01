using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Utilities;


namespace iDAS.Web.Areas.Profile.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý danh mục hồ sơ", Icon = "PageLink", IsActive = true, IsShow = true, Position = 2, Parent = GroupCategoryController.Code)]
    public class ProfileCategoryController : BaseController
    {
        ProfileCategoryService profileCategoryService = new ProfileCategoryService();
        ProfileService profileSV = new ProfileService();
        ProfileBorrowSV profileBorrowSV = new ProfileBorrowSV();
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
            var obj = profileCategoryService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                string ck = checkvalidDelete(id);
                if (ck.Equals(""))
                {
                    profileCategoryService.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StProfileCate").Reload();
                }
                else
                    Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
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
            var obj = profileCategoryService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        public ActionResult ShowFrmProfile(int id = 0)
        {
            if (id > 0)
            {
                var obj = profileCategoryService.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileView", Model = obj, ViewData = new ViewDataDictionary { { "DepartmentID", obj.DepartmentID } } };
            }
            return null;
        }
        public ActionResult LoadCate(StoreRequestParameters parameters, int groupId)
        {
            int totalCount;
            var lst = profileCategoryService.GetAll(parameters.Page, parameters.Limit, out totalCount, groupId);
            return this.Store(lst, totalCount);
        }
        public ActionResult LoadProfileByCate(StoreRequestParameters parameters, int cateId)
        {
            int totalCount;
            var lst = profileSV.GetAllByCateID(parameters.Page, parameters.Limit, out totalCount, cateId);
            return this.Store(lst, totalCount);
        }
        public ActionResult InsertCate(ProfileCategoryItem obj)
        {
            string ck = checkvalid(obj);
            if (!ck.Equals(""))
            {
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            }
            else
            {
                obj.CreateBy = User.ID;
                profileCategoryService.Insert(obj);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("StProfileCate").Reload();
                X.GetCmp<Window>("winNewCate").Close();
            }
            return this.Direct();
        }
        public ActionResult Update(ProfileCategoryItem obj)
        {
            string ck = checkvalid(obj, obj.ID);
            if (!ck.Equals(""))
            {
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            }
            else
            {
                obj.UpdateBy = User.ID;
                profileCategoryService.Update(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StProfileCate").Reload();
                X.GetCmp<Window>("winEditCate").Close();
            }
            return this.Direct();
        }

        public ActionResult InsertProfileBorrow(ProfileBorrowItem obj)
        {

            string ck = checkvalidBorrow(obj);
            if (!ck.Equals(""))
            {
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            }
            else
            {
                obj.CreateBy = User.ID;
                profileBorrowSV.Insert(obj);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("stProfiles").Reload();
                X.GetCmp<Window>("winEditBorrowProfile").Close();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Mượn hồ sơ")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmFromProfile(int id, int type = 0)
        {
            if (type == (int)ProfileForm.Borrow)//Thu hồi tài liệu
            {
                var objProfile = profileSV.GetByID(id);//Thông tin hồ sơ
                var objborrow = new ProfileBorrowItem { ProfileID = objProfile.ID, CategoryID = objProfile.ID, DepartmentID = objProfile.ReceivedDepartmentID, Name = objProfile.Name };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Borrow", Model = objborrow };
            }
            else if (type == (int)ProfileForm.Paid)//Thu hồi tài liệu
            {
                var objProfileBrr = profileBorrowSV.GetByProfileID(id);//Thông tin hồ sơ

                return new Ext.Net.MVC.PartialViewResult { ViewName = "Paid", Model = objProfileBrr };

            }
            return null;

        }
        public ActionResult UpdateProfilePaid(ProfileBorrowItem obj)
        {
            string ck = checkvalidBorrow(obj);
            if (!ck.Equals(""))
            {
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            }
            else
            {
                obj.UpdateBy = User.ID;
                profileBorrowSV.UpdatePaid(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("stProfiles").Reload();
                X.GetCmp<Window>("winPaidProfile").Close();
            }
            return this.Direct();
        }
        private string checkvalidBorrow(ProfileBorrowItem obj, int id = 0)
        {
            if (obj.LimitAt < obj.BorrowAt)
                return "Ngày hẹn trả phải lớn hơn hoặc bằng Ngày mượn!";
            if (obj.ReturnAt != null && obj.ReturnAt > DateTime.MinValue && obj.ReturnAt < obj.BorrowAt)
                return "Ngày trả phải lớn hơn hoặc bằng Ngày mượn Hồ sơ!";
            return "";
        }
        private string checkvalid(ProfileCategoryItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();
            if (id > 0)
            {
                var docItem = profileCategoryService.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = profileCategoryService.GetByNameDepartment(objDraft.Name, objDraft.DepartmentID);
                if (doc != null)
                {
                    return "Tên danh mục Hồ sơ này đã tồn tại trong phòng ban " + objDraft.Department + "! Vui lòng nhập lại tên danh mục!";
                }
            }
            return "";
        }
        private string checkvalidDelete(int id)
        {

            if (id > 0)
            {
                var docItem = profileSV.GetAllByCateID(id);
                if (docItem != null && docItem.Count() > 0)
                {
                    return "Danh mục này đang chứa hồ sơ nên không thể xóa!";
                }
            }
            return "";
        }
    }
}
