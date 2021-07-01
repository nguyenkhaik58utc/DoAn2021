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
    [BaseSystem(Name = "Tự đánh giá", Icon = "User", IsActive = true, IsShow = true, Position = 2, Parent = GroupRecruitmentTrialController.Code)]
    public class ProfileWorkTrialAuditEmployeeController : BaseController
    {
        //
        // GET: /Human/ProfileWorkTrialAuditEmployee/
        private HumanProfileWorkTrialSV service = new HumanProfileWorkTrialSV();
        #region Lấy dữ liệu
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = service.GetByEmployee(filter, User.EmployeeID, focusId);
            return this.Store(lstData, filter.PageTotal);
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
        #endregion
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
    }
}
