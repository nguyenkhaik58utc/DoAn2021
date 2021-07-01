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



namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Quản lý đánh giá", IsActive = true, IsShow = false, Position = 8, Parent = GroupRecruitmentController.Code)]
    public class RecruitmentReviewController : BaseController
    {
        private HumanRecruitmentReviewSV ReviewService = new HumanRecruitmentReviewSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        #region Thêm
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(string Name, int ID = 0, int RequirementID = 0)
        {
            var data = new HumanRecruitmentProfileInterviewItem();
            data.ID = ID;
            data.RequirementID = RequirementID;
            data.ProfileName = Name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        /// <summary>
        /// Lấy danh sách hồ sơ xét tuyển của kế hoạch
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadProfileInterview(StoreRequestParameters parameters, int PlanID = 0, int RequirementID = 0)
        {
            int totalCount;
            var result = ReviewService.GetByPlanIDAndRequirementID(parameters.Page, parameters.Limit, out totalCount, PlanID, RequirementID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// Lấy danh sách các kết quả các tiêu chí cho nhân sự 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="ProfileInterviewID"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns>
        public ActionResult LoadCriteria(StoreRequestParameters parameters, int ProfileInterviewID, int RequirementID)
        {
            int totalCount;
            var result = ReviewService.GetCriteria(parameters.Page, parameters.Limit, out totalCount, ProfileInterviewID, RequirementID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// Cập nhật kết quả cho các tiêu chí
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult UpdateReview(string ReviewItem)
        {
            try
            {
                var updateData = Ext.Net.JSON.Deserialize<HumanRecruitmentReviewItem>(ReviewItem);
                ReviewService.UpdateOrInsert(updateData, User.ID);
                X.GetCmp<Store>("StoreCriteria").GetById(updateData.CriteriaID).Commit();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion


        
    }
}
