using iDAS.Core;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Web.Areas.Human.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Util.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Lãnh đạo đánh giá", Icon = "UserTick", IsActive = true, IsShow = true, Position = 3,
                    Parent = GroupAuditPerformController.Code)]
    public class AuditLeaderController : BaseController
    {
        private HumanAuditTickSV HumanAuditTickSV = new HumanAuditTickSV();
        private HumanAuditTickResultSV HumanAuditTickResultSV = new HumanAuditTickResultSV();

        #region Lấy dữ liệu
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetGradation(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = HumanAuditTickSV.GetGradationByLeader(filter, User.EmployeeID);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetData(StoreRequestParameters parameters, int gradationId = 0, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = HumanAuditTickSV.GetByLeader(filter, User.EmployeeID, gradationId, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        #endregion

        #region Lãnh đạo đánh giá
        public ActionResult Answer(int id = 0)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Answer",
                Model = new HumanAuditTickItem() { ID = id },
                ViewData = ViewData
            };
        }

        public ActionResult GetAnswer(int id = 0)
        {
            var lstData = HumanAuditTickResultSV.GetAnswer(id);
            return this.Store(lstData);
        }

        public DirectResult HandleChanges(int id, int criteriaPointID)
        {
            HumanAuditTickResultSV.UpdateLeaderResult(id, criteriaPointID, User.EmployeeID);
            X.GetCmp<Store>("stAuditAnswer").Reload();
            return this.Direct();
        }
        public ActionResult Approval(int ID, decimal EmployeeAuditResult, decimal EmployeeAuditManagementResult, decimal EmployeeAuditLeaderResult)
        {
            try
            {
                HumanAuditTickSV.Approval(ID, EmployeeAuditResult, EmployeeAuditManagementResult, EmployeeAuditLeaderResult, User.ID);
                Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError);
            }
            return this.Direct();
        }
        #endregion

    }
}
