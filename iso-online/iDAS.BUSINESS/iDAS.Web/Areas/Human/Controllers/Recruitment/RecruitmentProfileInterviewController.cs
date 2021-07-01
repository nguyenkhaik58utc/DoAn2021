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
    [BaseSystem(Name = "Hồ sơ ứng tuyển", Icon = "UserAdd", IsActive = true, IsShow = true, Position = 2, Parent = GroupRecruitmentProfileController.Code)]
    public class RecruitmentProfileInterviewController : BaseController
    {
        private HumanRecruitmentProfileInterviewSV recruitmentProfileInterviewService = new HumanRecruitmentProfileInterviewSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }

        #region Thêm
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm(int ID, string Name)
        {
            var data = new HumanRecruitmentPlanItem();
            data.ID = ID;
            data.Name = Name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Add", Model = data };
        }
        /// <summary>
        /// Lấy danh sách yêu cầu theo mã kế hoạch
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public ActionResult GetRequirement(int PlanID = 0)
        {
            var Requirements = new List<HumanRecruitmentPlanRequirementItem>();
            if (PlanID != 0)
            {
                Requirements = recruitmentProfileInterviewService.GetRequirementByPlan(PlanID);
            }
            return this.Store(Requirements);
        }
        /// <summary>
        /// Lấy danh sách hồ sơ
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadProfile(StoreRequestParameters parameters, int PlanID = 0)
        {
            if (PlanID != 0)
            {
                int totalCount;
                //var result = recruitmentProfileInterviewService.GetAllByPlanID(parameters.Page, parameters.Limit, out totalCount, PlanID);
                var result = recruitmentProfileInterviewService.GetAllNotIsEmpByPlanID(parameters.Page, parameters.Limit, out totalCount, PlanID);
                return this.Store(result, totalCount);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult UpdateProfileInterView(string data, int PlanID = 0)
        {
            var ProfileInterviewdata = Ext.Net.JSON.Deserialize<RecruitmentProfileIneterviewSelectItem>(data);
            try
            {
                if (ProfileInterviewdata.IsSelect == true)
                {
                    var ProfileInterviewtem = new HumanRecruitmentProfileInterviewItem();
                    if (ProfileInterviewdata.RequirementID == null)
                    {

                        ProfileInterviewtem.ProfileID = ProfileInterviewdata.ProfileID;
                        ProfileInterviewtem.PlanID = PlanID;
                        ProfileInterviewtem.RequirementID = 0;
                        recruitmentProfileInterviewService.InsertOrDefaulRequirment(ProfileInterviewtem, User.ID, PlanID);
                    }
                    else
                    {
                        ProfileInterviewtem.ProfileID = ProfileInterviewdata.ProfileID;
                        ProfileInterviewtem.RequirementID = (int)ProfileInterviewdata.RequirementID;
                        recruitmentProfileInterviewService.UpdateRequirement(ProfileInterviewtem.ProfileID, ProfileInterviewtem.RequirementID);
                    }
                }
                else
                {
                    if (ProfileInterviewdata.ID != null)
                    {
                        recruitmentProfileInterviewService.Delete((int)ProfileInterviewdata.ID);
                    }
                    else
                    {
                        var ProfileInterviewtem = new HumanRecruitmentProfileInterviewItem();
                        ProfileInterviewtem.ProfileID = ProfileInterviewdata.ProfileID;
                        ProfileInterviewtem.PlanID = PlanID;
                        ProfileInterviewtem.RequirementID = (int)ProfileInterviewdata.RequirementID;
                        recruitmentProfileInterviewService.InsertOrDefaulRequirment(ProfileInterviewtem, User.ID, PlanID);
                    }
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("ProfileAddStore").Reload();
                X.GetCmp<Store>("StoreProfile").Reload();
            }
            return this.Direct();
        }
        #endregion



        #region Chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ProfileInterviewID = 0)
        {
            var data = new HumanRecruitmentResultItem();
            recruitmentProfileInterviewService.GetResultByProfileInterViewID(ProfileInterviewID, out data);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        /// <summary>
        /// hiển thị chi tiết cho kết quả tuyển dụng 
        /// </summary>
        /// <param name="ProfileResultID"></param>
        /// <returns></returns>
        public ActionResult DetailResultForm(int ProfileResultID = 0)
        {
            var data = recruitmentProfileInterviewService.GetResultByID(ProfileResultID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        /// <summary>
        /// Lấy danh sách hồ sơ phỏng vấn của kế hoạch
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadProfileInterview(StoreRequestParameters parameters, int PlanID = 0, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = recruitmentProfileInterviewService.GetProfileByPlan(filter, focusId, PlanID);
            return this.Store(result, filter.PageTotal);
        }
        #endregion

        #region Phê duyệt
        /// <summary>
        /// Trỏ đến form  gửi duyệt
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult SendApproveForm(int ProfileInterviewID = 0)
        {
            var data = new HumanRecruitmentResultItem();
            recruitmentProfileInterviewService.GetResultByProfileInterViewID(ProfileInterviewID, out data);
            data.ActionForm = Form.Send;
            if (data.TotalPoint == null)
            {
                Ultility.ShowMessageBox(title: RequestMessage.Notify, message: RequestMessage.NotReview);
                return this.Direct();
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SendApprove", Model = data };
        }
        /// <summary>
        /// Trỏ đến form phê duyệt
        /// </summary>
        /// <param name="ProfileInterviewID"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns>
        public ActionResult ApproveForm(int ProfileResultID = 0)
        {
            var data = recruitmentProfileInterviewService.GetResultByID(ProfileResultID);
            data.ActionForm = Form.Approve;
            if (data.TotalPoint == null)
            {
                Ultility.ShowMessageBox(title: RequestMessage.Notify, message: RequestMessage.NotReview);
                return this.Direct();
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
        }
        /// <summary>
        /// Xử lý thông tin gửi duyệt
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult SendApprove(HumanRecruitmentResultItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.TotalPoint == null)
                {
                    Ultility.ShowMessageBox(title: RequestMessage.Notify, message: RequestMessage.NotReview);
                    return this.Direct();
                }
                recruitmentProfileInterviewService.SendProfile(updateData, User.ID);
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một hồ sơ ứng tuyển cần phê duyệt", updateData.CreateByName, updateData.Approval.ID, User, Common.RecruitmentProfileInterview, "RecruitmentProfileInterviewID:" + updateData.ID.ToString());
                    
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfile").Reload();
            }
            return this.FormPanel(success);
        }
        /// <summary>
        /// Thực hiện phê duyệt kế hoạch tuyển dụng
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult ApproveReview(HumanRecruitmentResultItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    updateData.IsAccept = updateData.IsResult == true ? true : false;
                    updateData.IsApproval = true;
                    recruitmentProfileInterviewService.UpdateResult(updateData, User.ID);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Hồ sơ ứng tuyển của bạn đã được phê duyệt", updateData.CreateByName, updateData.CreateUserID, User, Common.RecruitmentProfileInterview, "RecruitmentProfileInterviewID:" + updateData.ID.ToString());
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfileResult").Reload();

            }
            return this.FormPanel(success);
        }
        #endregion

    }
}
