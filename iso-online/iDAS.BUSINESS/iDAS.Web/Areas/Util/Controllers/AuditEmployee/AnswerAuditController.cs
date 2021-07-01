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
    [BaseSystem(Name = "Trả lời câu hỏi đánh giá năng lực", Icon = "UserComment", IsActive = true, IsShow = true, Position = 3, Parent = GroupAuditEmployeeController.Code)]
    public class AnswerAuditController : BaseController
    {
        //
        // GET: /Human/AnswerAudit/
        private HumanPhaseAuditSV humanPhaseAuditSV = new HumanPhaseAuditSV();
        private HumanResultQuestionSV humanResultQuestionSV = new HumanResultQuestionSV();
        private HumanEmployeeAuditSV humanEmployeeAuditSV = new HumanEmployeeAuditSV();
        private HumanAnswerSV humanAnswerSV = new HumanAnswerSV();
        private HumanQuestionSV humanQuestionSV = new HumanQuestionSV();
        private HumanResultAnswerSV humanResultAnswerSV = new HumanResultAnswerSV();
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanPhaseAuditItem> lstData;
            lstData = humanPhaseAuditSV.GetByEmployeeID(filter, User.EmployeeID, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult LoadDataAnswer(StoreRequestParameters parameters, int questionId = 0, int humanResultQuestionID = 0)
        {
            List<HumanResultAnswerItem> lstData;
            lstData = humanResultAnswerSV.GetByCateId(questionId, humanResultQuestionID);
            return this.Store(lstData);
        }
        public ActionResult GetQuestionAnswer(StoreRequestParameters parameters, int cateId = 0, int phaseAuditID = 0)
        {
            List<HumanQuestionItem> lstData;
            var humanEmployeeAuditId = humanEmployeeAuditSV.GetByPhaseAuditIDAndEmployeeID(phaseAuditID, User.EmployeeID);
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = humanQuestionSV.GetByCateIsUse(filter, cateId, phaseAuditID, humanEmployeeAuditId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult ShowAnswer(int id = 0, int phaseAuditID = 0)
        {
            if (id > 0)
            {
                var obj = humanQuestionSV.GetById(id);
                var humanEmployeeAuditId = humanEmployeeAuditSV.GetByPhaseAuditIDAndEmployeeID(phaseAuditID, User.EmployeeID);
                var humanResultQuestionID = humanResultQuestionSV.GetIDByHumanEmployeeAuditIDandHumanQuestionID(humanEmployeeAuditId, id);
                ViewData["QuestionID"] = id;
                ViewData["HumanEmployeeAuditID"] = humanEmployeeAuditId;
                ViewData["HumanResultQuestionID"] = humanResultQuestionID;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Answer", Model = obj, ViewData = ViewData };
            }
            return null;
        }
        public ActionResult Answer(int id = 0, int phaseAuditID = 0)
        {
            if (humanPhaseAuditSV.GetByID(id).EndDate <= DateTime.Now)
            {
                if (id != 0)
                {
                    ViewData["Audit"] = id;
                    ViewData["PhaseAuditID"] = phaseAuditID;
                }
                var humanEmployeeAuditId = humanEmployeeAuditSV.GetByPhaseAuditIDAndEmployeeID(phaseAuditID, User.EmployeeID);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Answer", Model = humanQuestionSV.GetByCateId(id, phaseAuditID, humanEmployeeAuditId), ViewData = ViewData };
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Thời gian trả lời câu hỏi đã hết hạn!"
                });
                return this.Direct();
            }
          
        }
        public ActionResult SaveAnswerQuestion(int phaseAuditId = 0)
        {
            var uids = Request.Params.AllKeys.Where(a => a.Contains("uId"));
            if (uids != null)
            {
                var t = uids.Select(a => System.Convert.ToInt32(Request.Params[a])).ToArray();
                if (t != null && t.Count() > 0)
                {
                    try
                    {
                        var objNew = new HumanResultQuestionItem();
                        humanResultQuestionSV.InsertResultQuestion(objNew, t, phaseAuditId, User.ID, User.EmployeeID);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        return this.RedirectToActionPermanent("Index");
                    }
                    catch
                    {
                        Ultility.CreateNotification(message: RequestMessage.CreateError);
                        return this.RedirectToActionPermanent("Index");
                    }
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateError);
                    return this.RedirectToActionPermanent("Index");
                }
            }
            return this.RedirectToActionPermanent("Index");
        }

    }
}
