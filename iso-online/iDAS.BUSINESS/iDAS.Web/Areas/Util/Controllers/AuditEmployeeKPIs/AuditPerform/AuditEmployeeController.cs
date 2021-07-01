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
    [BaseSystem(Name = "Tự đánh giá", Icon = "User", IsActive = true, IsShow = true, Position = 1,
                    Parent = GroupAuditPerformController.Code)]
    public class AuditEmployeeController : BaseController
    {
        private HumanAuditTickResultSV HumanAuditTickResultSV = new HumanAuditTickResultSV();
        private HumanAuditTickSV HumanAuditTickSV = new HumanAuditTickSV();

        #region Lấy dữ liệu
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = HumanAuditTickSV.GetByEmployee(filter, User.EmployeeID, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        #endregion

        #region Tự đánh giá
        public ActionResult Answer(int id = 0)
        {
            ViewData["AuditTickId"] =id;
            var lst = HumanAuditTickSV.GetCriteriaByTickID(id);
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Answer",
                Model = lst,
                ViewData = ViewData
            };
        }
        public ActionResult SaveAnswerQuestion(int auditTickId = 0)
        {
            var uids = Request.Params.AllKeys.Where(a => a.Contains("uId"));
            if (uids != null)
            {
                var ids = uids.Select(a => System.Convert.ToInt32(Request.Params[a])).ToArray();
                var answers = new List<HumanAuditTickResultItem>();
                if (ids != null && ids.Count() > 0)
                {
                    foreach (var id in ids)
                    {
                        answers.Add(new HumanAuditTickResultItem()
                        {
                            HumanAuditTickID = auditTickId,
                            AuditEmployeeResult = id
                        });
                    }
                    HumanAuditTickResultSV.InsertEmployeeAudits(answers, auditTickId, User.ID);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Bạn có một phiếu đánh giá nhân sự cần thực hiện", HumanAuditTickSV.GetNameGradation(auditTickId), HumanAuditTickSV.GetMangement(auditTickId), User, Common.AuditManagement, "AuditTickID:" + auditTickId.ToString());
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateError);
                    return this.RedirectToActionPermanent("Index");
                }
            }
            return this.RedirectToActionPermanent("Index");
        }
        #endregion

        #region Xem chi tiết đánh giá
        public ActionResult Detail(int id = 0)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Detail",
                Model = new HumanAuditTickItem() { ID = id },
                ViewData = ViewData
            };
        }
        public ActionResult GetDetail(int id = 0)
        {
            var lstData = HumanAuditTickResultSV.GetAnswerDetail(id);
            return this.Store(lstData);
        }
        #endregion

    }
}
