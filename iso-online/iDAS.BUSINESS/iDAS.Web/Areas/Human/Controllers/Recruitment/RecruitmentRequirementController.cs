using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Web.Controllers;



namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Đề nghị", Icon = "PageAdd", IsActive = true, IsShow = true, Position = 2, Parent = GroupRecruitmentController.Code)]
    public class RecruitmentRequirementController : BaseController
    {
        private HumanRecruitmentRequirementSV RequirementService = new HumanRecruitmentRequirementSV();

        #region Thông tin chung
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        /// <summary>
        /// Load hồ sơ yêu cầu tuyển dụng
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadRequirement(StoreRequestParameters parameters, int focusId = 0, int DepartmentID = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            return this.Store(RequirementService.GetAllByDeptID(filter, focusId, DepartmentID), filter.PageTotal);
        }
        #endregion

        #region Thêm
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Add" };
        }

        /// <summary>
        /// Thêm mới yêu cầu tuyển dụng
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(HumanRecruitmentRequirementItem updateData, bool IsEdit = true)
        {
            try
            {
                var Id = 0;
                updateData.IsEdit = IsEdit;
                RequirementService.Insert(updateData, User.ID, out Id);
                if (!IsEdit)
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một đề nghị tuyển dụng cần phê duyệt", updateData.CreateByName, updateData.Approval.ID, User, Common.RecruitmentRequirement, "RecruitmentRequirementID:" + updateData.ID.ToString());
                }
                Ultility.CreateNotification(message: IsEdit ? RequestMessage.CreateSuccess : RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: IsEdit ? RequestMessage.UpdateError : RequestMessage.SendError, error: true);
            }
            finally
            {

                X.GetCmp<Window>("winRequirementAdd").Close();
                X.GetCmp<Store>("StoreRequirement").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Sửa
        /// <summary>
        /// Trỏ đến form cập nhật yêu cầu tuyển dụng 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int ID = 0, string ActionForm = "")
        {
            var data = new HumanRecruitmentRequirementItem();
            if (ID != 0)
            {
                data = RequirementService.GetById(ID);
            }
            data.ActionForm = ActionForm;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };

        }
        /// <summary>
        /// Cập nhật yêu cầu tuyển dụng
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(HumanRecruitmentRequirementItem updateData)
        {
            try
            {
                if (updateData.ID != 0)
                {
                    RequirementService.Update(updateData, User.EmployeeID);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một đề nghị tuyển dụng cần phê duyệt", updateData.CreateByName, updateData.Approval.ID, User, Common.RecruitmentRequirement, "RecruitmentRequirementID:" + updateData.ID.ToString());
                    Ultility.CreateNotification(message: updateData.IsEdit ? RequestMessage.UpdateSuccess : RequestMessage.SendSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: updateData.IsEdit ? RequestMessage.UpdateError : RequestMessage.SendSuccess, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winRequirementUpdate").Close();
                X.GetCmp<Store>("StoreRequirement").Reload();

            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        /// <summary>
        /// Xóa yêu cầu tuyển dụng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteRequirement(int id)
        {
            try
            {

                RequirementService.DeleteRequirAndApprov(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirement").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Chi tiết
        /// <summary>
        /// Form chi tiết yêu cầu tuyển dụng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID)
        {
            var data = RequirementService.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        /// <summary>
        /// Lấy danh sách tiêu chí theo chức danh
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public ActionResult LoadCriteria(StoreRequestParameters parameters, int roleID)
        {
            var CriteriaSv = new HumanRecruitmentCriteriaSV();
            int totalCount;
            var result = CriteriaSv.GetByRole(parameters.Page, parameters.Limit, out totalCount, roleID);
            return this.Store(result, totalCount);
        }
        #endregion

        #region Phê duyệt
        [BaseSystem(Name = "Phê duyệt")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Approve(int ID = 0, string ActionForm = "")
        {
            var data = new HumanRecruitmentRequirementItem();
            if (ID != 0)
            {
                data = RequirementService.GetById(ID);
            }
            data.ActionForm = ActionForm;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
        }
        /// <summary>
        /// Phê duyệt yêu cầu
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approve(HumanRecruitmentRequirementItem updateData)
        {
            var success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    updateData.UpdateBy = User.ID;
                    RequirementService.Approve(updateData);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Đề nghị tuyển dụng của bạn đã được phê duyệt", updateData.CreateByName, updateData.CreateUserID, User, Common.RecruitmentRequirement, "RecruitmentRequirementID:" + updateData.ID.ToString());
                    success = true;
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                    X.GetCmp<Store>("StoreRequirement").Reload();
                    
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            return this.FormPanel(success);
        }
        #endregion
    }
}
