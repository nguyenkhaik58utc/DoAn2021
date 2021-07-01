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
    [BaseSystem(Name = "Đánh giá nhân sự thử việc", Icon = "UserKey", IsActive = true, IsShow = true, Position = 3, Parent = GroupRecruitmentTrialController.Code)]
    public class ProfileWorkTrialAuditManagerController : BaseController
    {
        //
        // GET: /Human/ProfileWorkTrialAuditManager/
        private HumanProfileWorkTrialSV service = new HumanProfileWorkTrialSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, string fromDate, string toDate,int focusId = 0)
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
            var result = service.GetAllByManagerID(filter, searchFromDate, searchToDate,User.EmployeeID,focusId);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult GetAuditResult(int humanProfileWorkTrialID=0)
        {
            List<HumanProfileWorkTrialResultItem> lstData = new List<HumanProfileWorkTrialResultItem>();
            if (humanProfileWorkTrialID>0)
            {
                lstData = service.GetResultAudit(humanProfileWorkTrialID);
            }

            return this.Store(lstData, lstData.Count);
        }
        [BaseSystem(Name = "Đánh giá", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AuditForm(int id)
        {
            var data = new HumanProfileWorkTrialItem();
            data = service.getByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Audit", Model = data };
        }
        public ActionResult Detail(int id)
        {
            var data = new HumanProfileWorkTrialItem();
            data = service.getByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        public ActionResult EditAuditForm(int id)
        {
            var data = new HumanProfileWorkTrialItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "EditAudit", Model = data };
        }
        public ActionResult DetailAuditForm(int id)
        {
            var data = new HumanProfileWorkTrialItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailAudit", Model = data };
        }
        public ActionResult GetQualityCriteria(StoreRequestParameters parameters, string categoryID)
        {
            List<QualityCriteriaItem> lstData = new List<QualityCriteriaItem>();
            int total;
            if (!string.IsNullOrEmpty(categoryID))
            {
                lstData = new QualityCriteriaSV().GetCriteriaUsedByCateIds(parameters.Page, parameters.Limit, out total, categoryID);
            }

            return this.Store(lstData, lstData.Count);

        }
        public ActionResult SaveCriteria(string humanProfileWorkTrialID, string strPoint)
        {
            try
            {
                List<QualityCriteriaItem> lstPointItem = new List<QualityCriteriaItem>();
                lstPointItem = Ext.Net.JSON.Deserialize<QualityCriteriaItem[]>(strPoint).ToList();
                int hptID = int.Parse(humanProfileWorkTrialID);
                foreach(var item in lstPointItem)
                {
                    var data= new HumanProfileWorkTrialResultItem(){
                        HumanProfileWorkTrialID = hptID,
                        QualityCriteriaID = item.ID,
                        ManagerPoint = item.MaxPoint,
                        Note = item.Note
                    };
                    service.InsertCriterialResult(data);
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateAudit(string strPoint)
        {
            try
            {
                List<HumanProfileWorkTrialResultItem> lstPointItem = new List<HumanProfileWorkTrialResultItem>();
                lstPointItem = Ext.Net.JSON.Deserialize<HumanProfileWorkTrialResultItem[]>(strPoint).ToList();
                foreach (var item in lstPointItem)
                {
                    var data = new HumanProfileWorkTrialResultItem()
                    {
                        ID = item.ID,
                        EmployeePoint = item.EmployeePoint,
                        ManagerPoint = item.ManagerPoint,
                        Note = item.Note
                    };
                    service.UpdateCriterialResult(data);
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        #region Gửi duyệt
        [BaseSystem(Name = "Gửi duyệt")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult SendApprove(int ID = 0)
        {
            var data = new HumanProfileWorkTrialItem();
            if (ID != 0)
            {
                data = service.getByID(ID);
            }
            if (data.IsEdit)
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "SendApprove", Model = data };
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult SendApprove(HumanProfileWorkTrialItem updateData, bool IsEdit = false)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    service.SendApproval(updateData);
                    Ultility.CreateNotification(message: updateData.IsEdit ? RequestMessage.UpdateSuccess : RequestMessage.SendSuccess);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một đề xuất tuyển dụng nhân sự cần phê duyệt", updateData.CreateByName, updateData.Approval.ID, User, Common.ProfileWorkTrialPropose, "ProfileWorkTrialID:" + updateData.ID.ToString());

                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
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
