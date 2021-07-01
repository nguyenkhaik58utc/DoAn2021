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
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Kế hoạch đánh giá", Icon = "ScriptEdit", IsActive = true, IsShow = true, Position = 2, Parent = GroupMenuAuditController.Code)]
    public class AuditPlanController : BaseController
    {
        //
        // GET: /Quality/QualityAudit/
        private QualityAuditPlanSV auditPlanSV = new QualityAuditPlanSV();
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
            var lstData = auditPlanSV.GetAll(filter, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataIsApproval(StoreRequestParameters parameters)
        {
            List<QualityAuditPlanItem> lstData;
            int total;
            lstData = auditPlanSV.GetAllIsApproval(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<QualityAuditPlanItem>(lstData, total));
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create" };
        }

        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowUpdate(int id)
        {
            var obj = auditPlanSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Update", Model = obj };
        }
        public ActionResult ShowUpdateResultISO(int id)
        {
            var obj = auditPlanSV.GetResultISO(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ResultISO", Model = obj };
        }
        public ActionResult UpdateResultISO(QualityAuditPlanItem obj)
        {
            try
            {
                auditPlanSV.UpdateResultISO(obj, User.ID, User.EmployeeID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess, error: false);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        public ActionResult ShowApprove(int id)
        {
            var obj = auditPlanSV.GetById(id);
            if (obj.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt kế hoạch này!"
                });
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Approve", Model = obj };
            }
            return this.Direct();
        }
        public ActionResult SendApprove(QualityAuditPlanItem obj)
        {
            try
            {
                auditPlanSV.SendApprove(obj, User.ID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một kế hoạch đánh giá nội bộ yêu cầu phê duyệt", obj.Name, obj.Approval.ID, User, Common.AuditPlan, "AuditPlanID:" + obj.ID.ToString());
                X.GetCmp<Store>("stMnQualityAuditPlan").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        public ActionResult ApproveAudit(QualityAuditPlanItem obj)
        {
            try
            {
                auditPlanSV.Approve(obj, User.ID);
                if (obj.CreateByEmployeeID.HasValue)
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một kế hoạch đánh giá nội bộ đã được phê duyệt", obj.Name, obj.CreateByEmployeeID.Value, User, Common.AuditPlan, "AuditPlanID:" + obj.ID.ToString());
                }
                X.GetCmp<Store>("stMnQualityAuditPlan").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        public ActionResult ShowSendApproval(int id)
        {
            var obj = auditPlanSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "SendApproval", Model = obj };
        }
        public ActionResult Insert(QualityAuditPlanItem objNew)
        {
            try
            {
                if (!objNew.Code.Trim().Equals("") && !auditPlanSV.CheckCodeExits(objNew.Code.Trim()))
                {

                    if (objNew.Approval == null || objNew.Approval.ID == 0)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt kế hoạch!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                    else
                    {
                        objNew.CreateBy = User.ID;
                        auditPlanSV.Insert(objNew, User.ID);
                        X.GetCmp<Store>("stMnQualityAuditPlan").Reload();
                        X.GetCmp<Window>("wdAddAuditPlan").Close();
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Mã kế hoạch đã tồn tại vui lòng nhập mã khác!"
                    });
                    return this.Direct("ErrorExited");
                }

            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
            }
            return this.Direct();
        }
        public ActionResult Update(QualityAuditPlanItem objEdit)
        {
            try
            {
                if (!objEdit.Code.Trim().Equals("") && auditPlanSV.CheckCodeExitEdits(objEdit.ID, objEdit.Code.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Mã kế hoạch đã tồn tại vui lòng nhập mã khác!"
                    });
                }
                else
                {
                    if (objEdit.Approval == null || objEdit.Approval.ID == 0)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt kế hoạch!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                    else
                    {
                        auditPlanSV.Update(objEdit, User.ID);
                        X.GetCmp<Store>("stMnQualityAuditPlan").Reload();
                        X.GetCmp<Window>("wdEditAuditPlan").Close();
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    }
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                auditPlanSV.Delete(stringId);
                X.GetCmp<Store>("stMnQualityAuditPlan").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Kế hoạch đã được thiết lập không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = auditPlanSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
    }
}
