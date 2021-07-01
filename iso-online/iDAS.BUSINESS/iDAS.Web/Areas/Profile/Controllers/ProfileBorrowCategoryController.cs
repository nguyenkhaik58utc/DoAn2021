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
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Profile.Controllers
{
    [BaseSystem(Name = "Sổ theo dõi mượn hồ sơ", Icon = "Book", IsActive = true, IsShow = true, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProfileBorrowCategoryController : BaseController
    {
        private ProfileBorrowCategoryService profileBorowCateSV = new ProfileBorrowCategoryService();
        private ProfileBorrowSV profileBorrowSV = new ProfileBorrowSV();
        private ProfileService profileSV = new ProfileService();
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
        public ActionResult ShowFrmUpdate(int id = 0, int type = 0)
        {

            var obj = profileBorowCateSV.GetByID(id);
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
                    profileBorowCateSV.Delete(id);
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
            var obj = profileBorowCateSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        public ActionResult ShowFrmBorrow(int id, int type = 0, int cateID = 0)
        {
            var objProfile = profileBorrowSV.GetByID(id);

            if (type == (int)ProfileForm.BorrowByCate && cateID > 0)//Thu hồi tài liệu
            {
                var obj = profileBorowCateSV.GetByID(cateID);
                var objborrow = new ProfileBorrowItem { CategoryID = obj.ID, DepartmentID = obj.DepartmentID };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "BorrowByCate", Model = objborrow };
            }
            else if (type == (int)ProfileForm.Borrow)//Thu hồi tài liệu
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Borrow", Model = objProfile };

            }
            else if (type == (int)ProfileForm.UpdateBorrow)//Câp nhật thông tin Mượn/trả hồ sơ
            {
                if (objProfile.ReturnAt != null && objProfile.ReturnAt > System.DateTime.MinValue)
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "Paid", Model = objProfile };
                else
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "BorrowUpdate", Model = objProfile };
            }
            else if (type == (int)ProfileForm.Paid)//Thu hồi tài liệu
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Paid", Model = objProfile };
            }
            return null;
        }
        public ActionResult ShowProfileBorrow(int id)
        {
            var obj = profileBorowCateSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChooseProfileBorrow", Model = obj };
        }
        public ActionResult ShowProfile()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileBorrow" };
        }
        public ActionResult ShowFrmProfile(int id = 0)
        {
            if (id > 0)
            {
                var obj = profileBorowCateSV.GetByID(id);
                var idDep = 34;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DocumentView", Model = obj, ViewData = new ViewDataDictionary { { "DepartmentID", idDep } } };
            }
            return null;
        }
        [BaseSystem(Name = "Xem danh sách hồ sơ mượn")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmProfileBorrow(int id = 0, int depId = 0, bool isDetail = false)
        {
            if (id > 0)
            {
                var obj = new ProfileCategoryItem { ID = id, DepartmentID = depId };
                if (isDetail)
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileBorrowDetail", Model = obj };
                else
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileBorrow", Model = obj };
            }
            return null;

        }
        [BaseSystem(Name = "Khôi phục sổ mượn hồ sơ đã xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowProfileBorrowBackup(int id = 0)
        {
            if (id > 0)
            {
                var obj = new ProfileCategoryItem { ID = id, DepartmentID = id };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileBorrowBackup", Model = obj };
            }
            return null;

        }
        public ActionResult LoadCate(StoreRequestParameters parameters, int groupId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = profileBorowCateSV.GetAll(filter, groupId);
            return this.Store(lst);
        }
        public ActionResult LoadProfileByCate(StoreRequestParameters parameters, int cateID)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = profileBorowCateSV.GetAll(filter, cateID);
            return this.Store(lst);
        }
        public ActionResult LoadProfileBorrowDeleteByDept(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var lst = profileBorowCateSV.GetAllDeleteByGroup(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(lst, totalCount);
        }
        public ActionResult LoadProfileBorrowByCate(StoreRequestParameters parameters, int cateID)
        {
            int totalCount;
            var lst = profileBorrowSV.GetAllBorrow(parameters.Page, parameters.Limit, out totalCount, cateID);
            return this.Store(lst, totalCount);
        }
        public ActionResult LoadProfileBorrowByCateDepartment(StoreRequestParameters parameters, int cateID, int groupId)
        {
            int totalCount;
            var lst = profileSV.GetAllByDeptID(parameters.Page, parameters.Limit, out totalCount, cateID, groupId);
            return this.Store(lst, totalCount);
        }
        public ActionResult GetDataCboCate(int depID)
        {
            var lst = profileBorowCateSV.GetAllBorrowCate(depID);
            return this.Store(lst);
        }
        public ActionResult InsertCate(ProfileCategoryItem obj)
        {
            obj.CreateBy = User.ID;
            string ck = checkValid(obj);
            if (ck.Equals(""))
            {
                profileBorowCateSV.Insert(obj);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("StProfileCate").Reload();
                X.GetCmp<Window>("winNewCate").Close();
            }
            else
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            return this.Direct();
        }
        public ActionResult Update(ProfileCategoryItem obj)
        {
            obj.UpdateBy = User.ID;

            string ck = checkValid(obj, obj.ID);
            if (ck.Equals(""))
            {
                profileBorowCateSV.Update(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StProfileCate").Reload();
                X.GetCmp<Window>("winEditCate").Close();
            }
            else
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);

            return this.Direct();
        }
        public ActionResult BackupCate(int id)
        {
            profileBorowCateSV.UpdateDelete(id, User.ID);
            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            X.GetCmp<Store>("stProfileBorrowCateDelete").Reload();
            X.GetCmp<Store>("StProfileCate").Reload();
            return this.Direct();
        }
        public ActionResult UpdateProfilePaid(ProfileBorrowItem obj)
        {
            obj.UpdateBy = User.ID;
            string ck = checkvalidBorrow(obj);
            if (!ck.Equals(""))
            {
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            }
            else
            {
                profileBorrowSV.UpdatePaid(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("stProfileBorrowed").Reload();
                X.GetCmp<Window>("winPaidProfile").Close();
            }
            return this.Direct();
        }
        public ActionResult UpdateProfileBorrow(ProfileBorrowItem obj)
        {
            obj.UpdateBy = User.ID;
            string ck = checkvalidBorrow(obj);
            if (!ck.Equals(""))
            {
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            }
            else
            {
                profileBorrowSV.UpdateBorrow(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("stProfileBorrowed").Reload();
                X.GetCmp<Window>("winEditBorrowProfile1").Close();
            }
            return this.Direct();
        }
        public ActionResult InsertProfileBorrow(ProfileBorrowItem obj, string formName)
        {
            obj.CreateBy = User.ID;
            string ck = checkvalidBorrow(obj);
            if (!ck.Equals(""))
            {
                Ultility.CreateMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            }
            else
            {
                profileBorrowSV.Insert(obj);              
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("stProfileBorrowed").Reload();
                X.GetCmp<Window>(formName).Close();
            }
            return this.Direct();
        }
        private string checkValid(ProfileCategoryItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();

            //Ktra truong hop Ma TL da ton tai trong he thong

            if (id > 0)
            {
                var docItem = profileBorowCateSV.GetByID(id);
                if (docItem != null)
                {
                    //&&  docItem.Version.Trim().ToUpper().Equals(objDraft.Version.ToUpper())
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()))
                        ckexit = false;
                }
            }

            if (ckexit)
            {
                var doc = profileBorowCateSV.GetByName(objDraft.Name);
                if (doc != null)
                {
                    //Da ton tai
                    return "Tên sổ theo dõi đã tồn tại! Vui lòng nhập tên khác.";
                }
            }

            return "";

        }
        private string checkvalidBorrow(ProfileBorrowItem obj, int id = 0)
        {

            if (obj.LimitAt < obj.BorrowAt)
                return "Ngày hẹn trả phải lớn hơn hoặc bằng Ngày mượn!";
            if (obj.ReturnAt != null && obj.ReturnAt > DateTime.MinValue && obj.ReturnAt < obj.BorrowAt)
                return "Ngày trả phải lớn hơn hoặc bằng Ngày mượn hồ sơ!";

            return "";

        }
        private string checkvalidDelete(int id)
        {
            //Ktra truong hop Hồ sơ thuộc sổ theo dõi đã trả chưa
            if (id > 0)
            {
                var lst = profileBorrowSV.GetAllNotReturnByBorrowCate(id);
                if (lst != null && lst.Count() > 0)
                {
                    //Da ton tai
                    return "Sổ theo dõi đã chọn vẫn có Hồ sơ đang mượn chưa được trả, nên không thế xóa!";
                }
            }
            return "";
        }
    }
}
