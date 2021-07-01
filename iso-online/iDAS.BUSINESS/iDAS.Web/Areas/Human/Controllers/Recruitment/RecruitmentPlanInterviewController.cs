using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;


namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thiết lập vòng thi tuyển", IsActive = true, IsShow = false, Position = 3, Parent = GroupRecruitmentController.Code)]
    public class RecruitmentPlanInterviewController : BaseController
    {
        private HumanRecruitmentPlanInterviewSV interviewSV = new HumanRecruitmentPlanInterviewSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult InterviewForm(int ID)
        {
            var data = new HumanRecruitmentPlanInterviewItem();
            data.PlanID = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Interview", Model = data };
        }
        /// <summary>
        /// Gọi form cập nhật thêm sửa xóa cho vòng thi tuyển
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="action"></param>
        /// <param name="PlanID"></param>
        /// <param name="IsInterview"></param>
        /// <returns></returns>
        public ActionResult InterviewUpdateForm(int ID, string action, int PlanID)
        {
            var planInterviewServices = new HumanRecruitmentPlanInterviewSV();
            var data = new HumanRecruitmentPlanInterviewItem();
            if (ID != 0)
            {
                data = interviewSV.GetById(ID);
                data.PlanID = PlanID;

            }
            else
            {
                data.PlanID = PlanID;
            };
            data.ActionForm = action;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InterviewUpdate", Model = data };
        }
        /// <summary>
        /// Cập nhât vòng thi tuyển
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult UpdateInterview(HumanRecruitmentPlanInterviewItem updateData)
        {
            var planInterviewServices = new HumanRecruitmentPlanInterviewSV();
            try
            {
                if (updateData.ID != 0)
                {
                    interviewSV.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    interviewSV.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                X.GetCmp<Store>("PlanInterviewStore").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winInterviewUpdate").Close();
            }
            return this.Direct();
        }
        /// <summary>
        /// Lấy danh sách vòng thi tuyển của kế hoạch
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult LoadPlanInterview(StoreRequestParameters parameters, int ID)
        {
            var planInterviewServices = new HumanRecruitmentPlanInterviewSV();
            int totalCount;
            var result = interviewSV.GetByPlan(parameters.Page, parameters.Limit, out totalCount, ID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// Xóa vòng thi tuyển
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletePlanInterview(int id)
        {
            try
            {
                var planInterviewServices = new HumanRecruitmentPlanInterviewSV();
                interviewSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("PlanInterviewStore").Reload();
            }
            return this.Direct();
        }
    }
}
