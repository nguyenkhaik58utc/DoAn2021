using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using iDAS.Core;
using iDAS.Services;
using iDAS.Utilities;



namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Quản lý thi tuyển", IsActive = true, IsShow = false)]
    public class RecruitmentInterviewController : BaseController
    {
        private HumanRecruitmentInterviewSV InterviewService = new HumanRecruitmentInterviewSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(string Name,int ID=0,int PlanID=0)
        {
            var data = new HumanRecruitmentProfileInterviewItem();
            data.ID = ID;
            data.PlanID = PlanID;
            data.ProfileName = Name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        /// <summary>
        /// Lấy danh sách hồ sơ phỏng vấn của kế hoạch
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadProfileInterview(StoreRequestParameters parameters, int PlanID = 0, int RequirementID = 0)
        {
            int totalCount;
            var result = InterviewService.GetByPlanIDAndRequirementID(parameters.Page, parameters.Limit, out totalCount, PlanID, RequirementID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// lấy kết quả phỏng vấn của hồ sơ
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="ProfileInterviewID"></param>
        /// <returns></returns>
        public ActionResult LoadInterview(StoreRequestParameters parameters, int ProfileInterviewID = 0, int PlanID=0)
        {
            int totalCount;
            var result = InterviewService.GetByProfieInterviewID(parameters.Page, parameters.Limit, out totalCount, ProfileInterviewID,PlanID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// Xóa điểm thi
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteInterviewItem(int ID)
        {
            try
            {
                InterviewService.Delete(ID);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreInterview").Reload();
            }
            return this.Direct();
        }
        /// <summary>
        /// Cập nhật điểm thi của các vòng thi
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult UpdateInteview(string InterviewItem)
        {
            try
            {
                var updateData = Ext.Net.JSON.Deserialize <HumanRecruitmentInterviewItem>(InterviewItem);
                InterviewService.UpdateOrInsert(updateData, User.ID);
                X.GetCmp<Store>("StoreInterview").GetById(updateData.PlanInterviewID).Commit();
                
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }
        #region Chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID, string Name)
        {
            var data = new HumanRecruitmentPlanItem();
            data.ID = ID;
            data.Name = Name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
