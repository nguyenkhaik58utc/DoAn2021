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
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Util.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý đánh giá", Icon = "UserKey", IsActive = true, IsShow = true, Position = 2,
                    Parent = GroupAuditPerformController.Code)]
    public class AuditManagementController : BaseController
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
            var lstData = HumanAuditTickSV.GetGradationByManagement(filter, User.EmployeeID);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetData(StoreRequestParameters parameters, int gradationId, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = HumanAuditTickSV.GetByManagement(filter, User.EmployeeID, gradationId);
            return this.Store(lstData, filter.PageTotal);
        }
        #endregion

        #region Quản lý đánh giá
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

        public DirectResult HandleChanges(int id, int criteriaId, int criteriaPointID)
        {
            HumanAuditTickResultSV.UpdateManagementResult(id, criteriaPointID, User.EmployeeID);
            X.GetCmp<Store>("stAuditAnswer").Reload();
            return this.Direct();
        }

        public ActionResult SendLeader(int ID)
        {
            try
            {
                HumanAuditTickSV.SendLeader(ID, User.ID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Bạn có một phiếu đánh giá nhân sự cần thực hiện", HumanAuditTickSV.GetNameGradation(ID), HumanAuditTickSV.GetLeader(ID), User, Common.AuditLeader, "AuditTickID:" + ID.ToString());
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError);
            }
            return this.Direct();
        }
        #endregion



    }
}
