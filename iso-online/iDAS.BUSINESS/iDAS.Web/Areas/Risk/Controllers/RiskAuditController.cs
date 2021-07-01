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

namespace iDAS.Web.Areas.Risk.Controllers
{
    [BaseSystem(Name = "Cải tiến hoạt động", Icon = "FlagChecked", IsActive = true, IsShow = true, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskAuditController : BaseController
    {
        private RiskSV RiskSV = new RiskSV();
        private QualityNCSV NCSV = new QualityNCSV();
        private RiskAuditSV RiskAuditSV = new RiskAuditSV();
        private RiskControlSV RiskControlSV = new RiskControlSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadRiskAudit(StoreRequestParameters parameters, int dpId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = RiskSV.GetForAudit(filter, dpId);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult LoadRiskControl(StoreRequestParameters parameters, int riskId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = RiskControlSV.GetForAudit(filter, riskId);
            return this.Store(lst, filter.PageTotal);
        }
        [BaseSystem(Name = "Đánh giá", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm(int id = 0, int auditcontrolID=0)
        {
            var data = new RiskAuditItem();
            if (id != 0)
            {
                data = RiskAuditSV.GetByID(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.RiskControlID = auditcontrolID;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(RiskAuditItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    RiskAuditSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    RiskAuditSV.Insert(data, User.ID);                    
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }

            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new RiskAuditItem();
            data = RiskAuditSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        [BaseSystem(Name = "Thêm mới sự không phù hợp", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult RiskAuditNCUpdate(QualityNCItem item, int RiskControlID = 0)
        {
            bool success = false;
            try
            {
                if (item.ID != 0)
                {
                    NCSV.Update(item, User.ID);
                }
                else
                {
                    NCSV.InsertFromAuditRiskControl(item, RiskControlID);
                }
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }

        [BaseSystem(Name = "Xem lịch sử sự cố", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Incedent(int riskId)
        {
            ViewData["ID"] = riskId;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Incedent", ViewData = ViewData };
        }
    }
}
