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
    [BaseSystem(Name = "Danh sách biên bản hủy hồ sơ", Icon = "PageDelete", IsActive = true, IsShow = true, Position = 4)]
    public class ProfileCancelController : BaseController
    {
        private ProfileCancelService profileCancelService = new ProfileCancelService();
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
        public ActionResult ShowFrmUpdate(int id = 0)
        {
            var obj = profileCancelService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id = 0)
        {
            var obj = profileCancelService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Lựa chọn thành phần tham gia")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdateDepartment(int id = 0)
        {
            var obj = profileCancelService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateDepartment", Model = new ProfileCancelItem { CancelID = id, CancelAt = obj.CancelAt } };
        }
        [BaseSystem(Name = "Xem danh sách hồ sơ hủy")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmProfileCancel(int id = 0)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileCancel", Model = new ProfileCancelItem { ID = id } };
        }
        [BaseSystem(Name = "Lựa chọn hồ sơ hủy")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmChoseProfileCancel(int id = 0)
        {
            var obj = profileCancelService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChoseProfileCancel", Model = new ProfileCancelItem { CancelID = id, CancelAt = obj.CancelAt } };
        }
        public ActionResult ShowFrmProfile(int id = 0, int type = 0)
        {
            if (id > 0)
            {
                if (type == (int)ProfileForm.UpdateProfileCancel)
                {
                    var objP = profileCancelService.GetProfileByID(id);
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateProfileCancel", Model = objP };
                }
            }
            return null;
        }
        public ActionResult GetDataMethod()
        {
            ProfileCancelMethodService methodSV = new ProfileCancelMethodService();
            var lst = methodSV.GetAll();
            return this.Store(lst);
        }
        public ActionResult GetDataProfileCancel(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = profileCancelService.GetAll(filter);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult GetDataChoseProfileCancel(StoreRequestParameters parameters, int departmentID = 0, int id = 0)
        {
            int totalCount;
            var lst = profileSV.GetAllCancel(parameters.Page, parameters.Limit, out totalCount, departmentID, id);
            return this.Store(lst, totalCount);
        }
        public ActionResult GetObjByCancelProfile(int CancelId, int profileID)
        {
            var obj = profileCancelService.GetCancelByCancelIDProfileID(CancelId, profileID);
            var rs = new JsonResult();
            if (obj != null)
            {
                rs.Data = obj;
            }
            else
                rs.Data = new ProfileCancelItem { CancelID = CancelId, ProfileID = profileID };
            return rs;
        }
        public ActionResult GetDataProfileCancelByCancelID(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var lst = profileCancelService.GetAllByCancelID(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(lst, totalCount);
        }
        public ActionResult Insert(ProfileCancelItem obj)
        {
            obj.CreateBy = User.ID;
            string ck = checkValid(obj);
            if (ck.Equals(""))
            {
                profileCancelService.Insert(obj);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("stProfileCancel").Reload();
                X.GetCmp<Window>("winNewCancelProfile").Close();
            }
            else
            {
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck, 400);
            }
            return this.Direct();
        }
        public ActionResult LoadUsers(StoreRequestParameters parameters, int departmentID = 0, int cancelId = 0)
        {
            HumanEmployeeSV employeeService = new HumanEmployeeSV();
            int totalCount;
            var result = (departmentID == 0 || departmentID == 1) ? employeeService.GetAll(parameters.Page, parameters.Limit, out totalCount)
                : employeeService.GetAllByDepartmentId(parameters.Page, parameters.Limit, out totalCount, departmentID);
            profileCancelService.GetEmployeeByID(ref result, cancelId);
            return this.Store(result, totalCount);
        }
        public ActionResult LoadEmployeesByCancelID(StoreRequestParameters parameters, int cancelId)
        {
            int totalCount;
            var result = profileCancelService.GetEmployeeByCancelID(parameters.Page, parameters.Limit, out totalCount, cancelId);
            return this.Store(result, totalCount);
        }
        public ActionResult Update(ProfileCancelItem obj)
        {
            string ck = checkValid(obj, obj.ID);
            if (ck.Equals(""))
            {
                obj.UpdateBy = User.ID;
                profileCancelService.Update(obj);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("stProfileCancel").Reload();
                X.GetCmp<Window>("winEditCancelProfile").Close();
            }
            else
            {
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck, 400);

            }
            return this.Direct();
        }
        public ActionResult InsertCancel(ProfileCancelItem obj)
        {
            obj.UpdateBy = User.ID;
            if (updateProfileCancel(obj))
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("stProfileCancels").Reload();
                X.GetCmp<Store>("stProfileCancel").Reload();
                X.GetCmp<Window>("winChoseCancel").Close();
            }
            return this.Direct();
        }
        public ActionResult UpdateCancel(ProfileCancelItem obj)
        {
            obj.UpdateBy = User.ID;
            updateProfileCancel(obj);
            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            X.GetCmp<Store>("stProfileCancels").Reload();
            X.GetCmp<Store>("stProfileCancel").Reload();
            return this.Direct();
        }
        public ActionResult UpdateVerifyCancel(int id)
        {
            try
            {
                int updateBy = User.ID;
                profileCancelService.UpdateVerifyCancel(id, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("stProfileCancel").Reload();
            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError);
                throw;
            }
            return this.Direct();
        }
        public ActionResult UpdateEmployee(int cancelID, string data)
        {
            var user = Ext.Net.JSON.Deserialize<HumanEmployeeItem>(data);
            try
            {
                if (user.ID != null && user.IsCheck == true)
                {
                    profileCancelService.InsertEmployee(cancelID, user.ID, User.ID);

                }
                if (user.ID != null && user.IsCheck == false)
                {
                    profileCancelService.DeleteEmployee(cancelID, user.ID);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true).Show();
            }
            X.GetCmp<Store>("stEmployee").Reload();
            X.GetCmp<Store>("stProfileCancel").Reload();
            return this.Direct();
        }
        public ActionResult Delete(int id)
        {
            try
            {
                profileCancelService.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>("StProfileCate").Reload();
            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }
            return this.Direct();
        }
        public ActionResult DeleteProfileCancel(int id)
        {
            try
            {
                profileCancelService.DeleteProfileCancel(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>("stProfileCancels").Reload();
            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }
            return this.Direct();
        }
        public ActionResult DepartmentView(string treeID)
        {
            ViewData["TreeID"] = treeID;
            return PartialView();
        }
        private bool insert(ProfileCancelItem obj)
        {
            try
            {
                var id = profileCancelService.Insert(obj);
                if (!obj.DepartmentIDs.Trim().Trim(',').Equals("") && id > 0)
                {
                    int[] arID = iDAS.Utilities.Convert.ArrayIntbyString(obj.DepartmentIDs.Trim(), ',');
                    profileCancelService.InsertEmployees(id, arID, User.ID);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool updateProfileCancel(ProfileCancelItem obj)
        {
            try
            {
                if (obj.ID > 0)
                {
                    profileCancelService.UpdateProfileCancel(obj, User.ID);
                }
                else
                    profileCancelService.InsertProfileCancel(obj, User.ID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string checkValid(ProfileCancelItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();
            if (id > 0)
            {
                var docItem = profileCancelService.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = profileCancelService.GetByName(objDraft.Name);
                if (doc != null)
                {
                    return "Tên biên bản hủy hồ sơ đã tồn tại trong hệ thống! Vui lòng nhập tên khác.";
                }
            }
            return "";
        }
    }
}
