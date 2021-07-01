using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kết quả", Icon = "StarGold", IsActive = true, IsShow = true, Position = 5, Parent = GroupAuditKPIController.Code)]
    public class AuditTickResultController : BaseController
    {
        //
        // GET: /Human/ResultAudit/
        private HumanAuditGradationSV humanAuditGradationSV = new HumanAuditGradationSV();
        private HumanAuditTickResultSV humanAuditTickResultSV = new HumanAuditTickResultSV();
        private HumanAuditTickSV humanAuditTickSV = new HumanAuditTickSV();
        private HumanProfileAssessSV humanProfileAssessSV = new HumanProfileAssessSV();
        private HumanAuditLevelSV humanAuditLevelSV = new HumanAuditLevelSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadAuditPhase(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditGradationItem> lstData;
            lstData = humanAuditGradationSV.GetAll(filter, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataPoint(StoreRequestParameters parameters, int TickResultId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditTickResultBonusScoreItem> lstData;
            lstData = humanAuditTickResultSV.GetDataPointBonus(filter, TickResultId);
            return this.Store(lstData, filter.PageTotal);
        }

        public ActionResult LoadLevelData(int humanAuditDepartmentId = 0, int humanRoleId = 0)
        {
            List<ComboboxItem> lstData;
            lstData = humanAuditLevelSV.GetCombobox(humanAuditDepartmentId, humanRoleId);
            return this.Store(lstData);
        }
        public ActionResult GetData(StoreRequestParameters parameters, int departmentId = 0, int auditPhaseId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditTickItem> lstData;
            if (new DepartmentSV().GetByID(departmentId).ParentID == 0)
            {
                lstData = humanAuditTickSV.GetResult(filter, auditPhaseId);
            }
            else
            {
                lstData = humanAuditTickSV.GetResultByDepartment(filter, departmentId, auditPhaseId);
            }
            return this.Store(lstData, filter.PageTotal);
        }
        [BaseSystem(Name = "Cập nhật kết quả đánh giá")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id)
        {
            var obj = humanAuditTickSV.GetByID(id);
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Result", Model = obj };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult BonusPointForm(int id)
        {
            try
            {
                ViewData["id"] = id;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "BonusPointForm",ViewData = ViewData  };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Update(HumanAuditTickItem objEdit, string strPoint)
        {
            try
            {
                List<HumanAuditTickResultBonusScoreItem> lstPointItem = new List<HumanAuditTickResultBonusScoreItem>();
                lstPointItem = Ext.Net.JSON.Deserialize<HumanAuditTickResultBonusScoreItem[]>(strPoint).ToList();
                humanAuditTickSV.Update(objEdit, User.ID);
                humanAuditTickResultSV.UpdateDataPoint(lstPointItem,objEdit.ID, User.ID);
                return this.Direct();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertToProfile(int id)
        {
            try
            {
                var obj = humanAuditTickSV.GetByID(id);
                HumanProfileAssessItem item = new HumanProfileAssessItem();
                item.StartDate = obj.StartDate;
                item.EndDate = obj.EndDate;
                item.Name = obj.Name;
                item.EmployeeID = obj.HumanEmployeeID;
                item.EndDate = obj.EndDate;
                item.StartDate = obj.StartDate;
                item.Result = obj.LevelName;
                item.Note = obj.Comments;
                item.Score = obj.TotalLeaderAuditResult.ToString();
                humanProfileAssessSV.Insert(item, User.ID);
                humanAuditTickSV.UpdateIsSave(id, User.ID);
                X.GetCmp<Store>("StoreEmployeeAudtitResults").Reload();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
