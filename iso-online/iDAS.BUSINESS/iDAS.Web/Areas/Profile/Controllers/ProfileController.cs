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
    [BaseSystem(Name = "Quản lý thông tin hồ sơ", Icon = "Bookmark", IsActive = true, IsShow = true, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProfileController : BaseController
    {
        private ProfileService profileSV = new ProfileService();
        private ProfileCategoryService profileCategorySV = new ProfileCategoryService();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDataProfileInfo(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = profileSV.GetAll(filter);
            return this.Store(lst, filter.PageTotal);
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
                var obj = profileSV.GetByID(id);
                    if (obj.FileName != null && !obj.FileName.Equals("")) obj.FileName = "";
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id = 0)
        {
                var obj = profileSV.GetByID(id);              
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        //[BaseSystem(Name = "Xem danh sách")]
        //[SystemAuthorize(CheckAuthorize = true)]
        //public ActionResult ShowFrmDestroy(int id = 0)
        //{
        //        var obj = profileSV.GetByID(id);
        //            return new Ext.Net.MVC.PartialViewResult { ViewName = "Destroy", Model = obj };
        //}
        public ActionResult ShowFrmDocCate(int id = 0)
        {
            var obj = profileSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChoseProfileCate", Model = obj };
        }
        public ActionResult ShowDepartment()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Department" };
        }
        public ActionResult ShowProfileBorrowBackup(int id)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileBorrowBackup", Model = id };
        }
        public ActionResult GetDataCate()
        {
            var lst = profileCategorySV.GetAll();
            return this.Store(lst);
        }
        public ActionResult Insert(ProfileItem obj, [Bind(Prefix = "StorageFormID")]string[] borders)
        {
            obj.CreateBy = User.ID;
            obj.StorageFormInt = setDocIssForm(borders);
            int id = insert(obj);
            if (id > 0)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("stProfileInfo").Reload();
                return this.Direct(true);
            }
            else
            {
                return this.Direct(false);
            }
        }
        public ActionResult Update(ProfileItem obj, [Bind(Prefix = "StorageFormID")]string[] borders)
        {
            obj.UpdateBy = User.ID;
            obj.StorageFormInt = setDocIssForm(borders);
            if (update(obj))
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("stProfileInfo").Reload();
                return this.Direct(true);
            }
            else
            {
                return this.Direct(false);
            }
        }
        public ActionResult LoadCateByDepartment(StoreRequestParameters parameters, int departmentID = 0)
        {
            int totalCount;
            var lst = profileCategorySV.GetAll(parameters.Page, parameters.Limit, out totalCount, departmentID);
            return this.Store(lst, totalCount);
        }
        private int insert(ProfileItem objDraft)
        {
            try
            {
                int id = 0;
                string ck = checkValid(objDraft);
                if (ck.Equals(""))
                {
                    id = profileSV.Insert(objDraft);
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);
                return id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        private bool update(ProfileItem objDraft)
        {
            try
            {
                string ck = checkValid(objDraft, objDraft.ID);
                if (ck.Equals(""))
                {
                    objDraft.UpdateBy = User.ID;
                    profileSV.Update(objDraft);
                    return true;
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private int setDocIssForm(string[] doc)
        {
            if (doc != null)
            {
                if (doc.Count() == 2) return (int)StorageForm.Both;
                else
                {
                    if (doc[0].ToString().Equals(StorageForm.SoftCopy.ToString()))
                        return (int)StorageForm.SoftCopy;
                    else
                        return (int)StorageForm.HardCopy;
                }
            }
            return -1;
        }
        private string checkValid(ProfileItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Code = objDraft.Code.Trim();
            if (id > 0)
            {
                var docItem = profileSV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Code.Trim().ToUpper().Equals(objDraft.Code.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = profileSV.GetByCode(objDraft.Code);
                if (doc != null)
                {
                    return "Mã hồ sơ đã tồn tại trên hệ thống. Vui lòng nhập lại Mã hồ sơ!";
                }
            }
            return "";
        }
    }
}
