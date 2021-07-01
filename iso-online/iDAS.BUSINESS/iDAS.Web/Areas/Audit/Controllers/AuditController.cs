using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using Ext.Net.MVC;
using System.Web.Mvc;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Audit.Controllers
{
    public class AuditController : BaseController
    {
        private AuditSV auditService = new AuditSV();
        public ActionResult AuditUpdateWindow(int taskId = 0)
        {
            var obj = auditService.GetByTaskID(taskId, User.EmployeeID);
            var str = obj.ID == 0 ? "Lưu để tiếp tục" : "Lưu lại";
            ViewData["NameButtonSave"] = str;
            return new Ext.Net.MVC.PartialViewResult {ViewData =ViewData };
        }
        public ActionResult AuditDetailWindow(int id)
        {
            return new Ext.Net.MVC.PartialViewResult() { };
        }
        public ActionResult Edit(int taskId = 0)
        {
          var obj = auditService.GetByTaskID(taskId, User.EmployeeID);      
            return PartialView(obj);
        }
        public ActionResult Detail(int id = 0)
        {
            AuditItem obj = new AuditItem();
            obj = auditService.GetByID(id);
            if (id == 0)
            {
                obj.EmployeeID = User.EmployeeID;
            }
            return PartialView(obj);
        }
        public ActionResult UpdateAudit(AuditItem objAudit)
        {
            try
            {
                objAudit.EmployeeID = objAudit.Audit.ID;
                if (objAudit.EmployeeID != User.EmployeeID && !User.Administrator)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn không có quyền đánh giá công việc này", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else if (objAudit.ID != 0)
                {
                    auditService.Update(objAudit, User.ID, User.EmployeeID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct(true);
                }
                else
                {
                    int auditId = auditService.Insert(objAudit);
                    X.GetCmp<Hidden>("hdfAuditId").SetValue(auditId);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct(false);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
        }
        public ActionResult Result(int taskId = 0)
        {
            AuditItem obj = new AuditItem();
            obj = auditService.GetByTaskID(taskId, User.EmployeeID);
            return PartialView(obj);
        }
        public ActionResult ResultDetail()
        {
            return PartialView();
        }
        public ActionResult Delete(int id)
        {
            try
            {
                auditService.Delete(id);
                X.GetCmp<Store>("StoreAudit").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }

        }
    }
}
