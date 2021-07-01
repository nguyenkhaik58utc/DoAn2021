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
using System.IO;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Đề xuất tuyển dụng", Icon = "ApplicationSideList", IsActive = true, IsShow = true, Position = 3, Parent = GroupRecruitmentTrialController.Code)]
    public class ProfileWorkTrialProposeController : BaseController
    {
        //
        // GET: /Human/ProfileWorkTrialPropose/
        private HumanProfileWorkTrialSV service = new HumanProfileWorkTrialSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, string fromDate, string toDate, int focusId = 0)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = System.Convert.ToDateTime(fromDate);
            }
            else
            {
                searchFromDate = DateTime.Now;
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = System.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            else
            {
                searchToDate = DateTime.Now;
            }
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = service.GetAll(filter, searchFromDate, searchToDate);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult GetAuditResult(int humanProfileWorkTrialID = 0)
        {
            List<HumanProfileWorkTrialResultItem> lstData = new List<HumanProfileWorkTrialResultItem>();
            if (humanProfileWorkTrialID > 0)
            {
                lstData = service.GetResultAudit(humanProfileWorkTrialID);
            }

            return this.Store(lstData, lstData.Count);
        }
        #region Phê duyệt
        [BaseSystem(Name = "Phê duyệt")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Approve(int ID = 0)
        {
            var data = new HumanProfileWorkTrialItem();
            if (ID != 0)
            {
                data = service.getByID(ID);
            }
            if (!data.IsEdit)
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Approve(HumanProfileWorkTrialItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    updateData.IsAccept = updateData.IsResult == true;
                    service.Approve(updateData);
                    new HumanEmployeeSV().UpdateTrial(updateData.HumanEmployeeID,updateData.IsAccept);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Đề xuất nhân sự của bạn đã được phê duyệt", updateData.CreateByName, updateData.ManagerID.Value, User, Common.ProfileWorkTrialManager, "TrainingRequirementID:" + updateData.ID.ToString());
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
                X.GetCmp<Store>("stMnPhaseAudit").Reload();

            }
            return this.FormPanel(success);
        }
        #endregion
    }
}
