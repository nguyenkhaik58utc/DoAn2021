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
    [BaseSystem(Name = "Hồ sơ trúng tuyển", Icon = "UserTick", IsActive = true, IsShow = true, Position = 3, Parent = GroupRecruitmentProfileController.Code)]
    public class RecruitmentResultController : BaseController
    {
        private HumanRecruitmentResultSV ResultService = new HumanRecruitmentResultSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// danh sách hồ sơ trúng tuyển
        /// </summary>
        /// <returns></returns>
        public ActionResult ProfilePassForm(int ID, string Name)
        {
            var data = new HumanRecruitmentPlanItem();
            data.ID = ID;
            data.Name = Name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfilePass", Model = data };
        }
        /// <summary>
        /// Lấy danh sách hồ sơ trúng tuyển đã được phê duyệt
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadProfilePass(StoreRequestParameters parameters, int PlanID = 0, int RequirementID = 0)
        {
            int totalCount;
            var result = ResultService.GetProfilePass(parameters.Page, parameters.Limit, out totalCount, PlanID, RequirementID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// Lấy danh sách các hồ sơ được đề xuất
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="PlanID"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns> 
        public ActionResult LoadProfileResult(StoreRequestParameters parameters, int PlanID = 0)
        {
            int totalCount;
            var result = ResultService.GetProfileResultByPlan(parameters.Page, parameters.Limit, out totalCount, PlanID);
            return this.Store(result, totalCount);
        }
        /// <summary>
        /// Thêm mới nhân sự vào hệ thống
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <returns></returns>
        public ActionResult AddNewEmployee(int ProfileID = 0)
        {
            try
            {
                if (ProfileID != 0)
                {
                    ResultService.AddNewEmployee(ProfileID, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("StoreProfileResult").Reload();
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);

            }
            return this.Direct();
        }
        public ActionResult AddNewEmployeeTrialForm(int ProfileID, string Name)
        {
            var data = new HumanProfileWorkTrialItem();
            data.ProfileID = ProfileID;
            data.Name = Name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "AddEmployeeTrial", Model = data };
        }
        public ActionResult AddNewEmployeeTrial(HumanProfileWorkTrialItem data)
        {
            bool success = true;
            try
            {
                if (data.ProfileID != 0)
                {
                    data.Employee.ID = ResultService.AddNewEmployee(data.ProfileID, User.ID,true);
                    if (data.Employee.ID > 0)
                        new HumanProfileWorkTrialSV().Insert(data,User.EmployeeID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("StoreProfileResult").Reload();
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                success = false;
            }
            return this.Direct(success);
        }
    }
}
