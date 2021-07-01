using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Đợt kiểm tra đánh giá năng lực", Icon = "FingerPoint", IsActive = true, IsShow = true, Position = 2, Parent = GroupAuditEmployeeController.Code)]
    public class PhaseAuditController : BaseController
    {
        //
        // GET: /Human/PhaseAudit/
        private HumanPhaseAuditSV humanPhaseAuditSV = new HumanPhaseAuditSV();
        private HumanEmployeeAuditSV humanEmployeeAuditSV = new HumanEmployeeAuditSV();
        private HumanCategoryQuestionSV humanCategoryQuestionSV = new HumanCategoryQuestionSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult ShowSendApproval(int id)
        {
            var obj = humanPhaseAuditSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "SendApproval", Model = obj };
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanPhaseAuditItem> lstData;
            lstData = humanPhaseAuditSV.GetAll(filter, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataQuestion()
        {
            List<HumanCategoryQuestionItem> lstData;
            lstData = humanCategoryQuestionSV.GetCombobox();
            return this.Store(lstData);
        }
        public ActionResult GetDataEmployeeAudit(StoreRequestParameters parameters, int phaseAuditId)
        {
            List<HumanEmployeeAuditItem> lstData;
            int total;
            lstData = humanEmployeeAuditSV.GetAll(parameters.Page, parameters.Limit, out total, phaseAuditId);
            return this.Store(new Paging<HumanEmployeeAuditItem>(lstData, total));
        }
        public ActionResult FormListObject()
        {
            return View("LstObject");
        }
        public ActionResult ChoiceObject(int phaseAuditId = 0)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ChoiceObject", ViewData = new ViewDataDictionary { { "phaseAuditId", phaseAuditId } } };
        }
        public ActionResult SaveAuditObject(string stringId, int phaseAuditId)
        {
            try
            {
                if (stringId.Trim() != "")
                {
                    humanEmployeeAuditSV.InsertObject(stringId, User.ID, phaseAuditId);
                    X.GetCmp<Store>("stMnObject").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
        public ActionResult ChoiceObjectAdd(int phaseAuditId = 0)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ChoiceObjectAdd", ViewData = new ViewDataDictionary { { "phaseAuditId", phaseAuditId } } };
        }
        public ActionResult GetDataIsApproval(StoreRequestParameters parameters)
        {
            List<HumanPhaseAuditItem> lstData;
            int total;
            lstData = humanPhaseAuditSV.GetAllIsApproval(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<HumanPhaseAuditItem>(lstData, total));
        }
        public ActionResult ApprovePhaseAudit(HumanPhaseAuditItem obj)
        {
            try
            {
                humanPhaseAuditSV.Approve(obj, User.ID);
                List<int> lstEmployeesID = new List<int>();
                if (obj.UpdateByEmployeeID.HasValue && obj.UpdateByEmployeeID != obj.ApprovalBy)
                {
                    lstEmployeesID.Add(obj.UpdateByEmployeeID.Value);
                }
                if (obj.CreateByEmployeeID.HasValue && obj.CreateByEmployeeID != obj.ApprovalBy)
                {
                    lstEmployeesID.Add(obj.CreateByEmployeeID.Value);
                }
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một đợt đánh giá năng lực được phê duyệt", obj.Name, lstEmployeesID, User, Common.PhaseAudit, "PhaseAuditID:" + obj.ID.ToString());
                if (obj.IsAccept) { 
                var employeeIds = humanPhaseAuditSV.GetEmployeeAudit(obj.ID);
                    if(employeeIds!=null)
                    {
                        notify.Notify(this.ModuleCode, "Bạn có một đợt đánh giá cần hoàn thiện", obj.Name, employeeIds, User, Common.AnswerAudit, "PhaseAuditID:" + obj.ID.ToString());
                    }
                }
                X.GetCmp<Store>("stMnPhaseAudit").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        public ActionResult ShowApprove(int id)
        {
            var obj = humanPhaseAuditSV.GetByID(id);
            if (obj.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt đợt đánh giá này!"
                });
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Approve", Model = obj };
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create" };
        }
        public ActionResult SendApprove(HumanPhaseAuditItem obj)
        {
            try
            {
                if (obj.Approval.ID == 0)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Bạn phải chọn người phê duyệt đợt đánh giá năng lực!"
                    });
                    return this.Direct();
                }
                else
                {
                    humanPhaseAuditSV.SendApprove(obj, User.ID);
                    X.GetCmp<Store>("stMnPhaseAudit").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct(true);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowUpdate(int id)
        {
            var obj = humanPhaseAuditSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj };
        }
        public ActionResult Insert(HumanPhaseAuditItem objNew, bool val, string arrObject)
        {
                try
                {
                    var objectAudit = Ext.Net.JSON.Deserialize<object[]>(arrObject);
                    if (objNew.Approval == null || objNew.Approval.ID == 0)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt đợt đánh giá năng lực!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                    else if (objectAudit.Length == 0)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn đối tượng tham gia đánh giá!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                        return this.Direct();
                    }
                    else if (!objNew.Name.Trim().Equals("") && !humanPhaseAuditSV.CheckNameExits(objNew.Name.Trim()))
                    {
                        int phaseAuditId = humanPhaseAuditSV.Insert(objNew,  User.ID);
                        if (objectAudit.Length > 0)
                        {
                            var HumanEmployeeAuditItems = new List<HumanEmployeeAuditItem>();
                            foreach (var item in objectAudit)
                            {
                                var s = Ext.Net.JSON.Deserialize<HumanEmployeeAuditItem>(item.ToString());
                                s.HumanPhaseAuditID = phaseAuditId;
                                s.HumanEmployeeID = s.ID;
                                var HumanEmployeeAuditItem = new HumanEmployeeAuditItem();
                                HumanEmployeeAuditItem.HumanPhaseAuditID = phaseAuditId;
                                HumanEmployeeAuditItems.Add(s);
                            }
                            humanEmployeeAuditSV.InsertRange(HumanEmployeeAuditItems, User.ID);
                        }                       
                        if (val == true)
                        {
                            X.GetCmp<Window>("winNewPhaseAudit").Close();
                        }
                        if (!objNew.IsEdit)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu phê duyệt đợt đánh giá năng lực", objNew.Name, objNew.Approval.ID, User, Common.PhaseAudit, "PhaseAuditID:" + phaseAuditId.ToString());
                        }
                        X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!").Show();
                        X.GetCmp<Store>("stMnPhaseAudit").Reload();
                        X.GetCmp<GridPanel>("grMnPhaseAudit").Refresh();
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên dịch vụ đã tồn tại vui lòng nhập tên khác!"
                        });
                        return this.Direct("ErrorExited");
                    }
                    return this.Direct();
                }
                catch (Exception ex)
                {
                    return this.Direct("Error");
                }
        }
        public ActionResult Update(HumanPhaseAuditItem objEdit, bool isApp=true)
        {
            
                try
                {
                    if (objEdit.Approval.ID == 0 && !objEdit.IsEdit)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.WARNING,
                            Title = "Thông báo",
                            Message = "Bạn phải chọn người phê duyệt đợt đánh giá năng lực!"
                        });
                        return this.Direct();
                    }
                    else
                    {
                        if (!isApp)
                            objEdit.IsApproval = false;
                        humanPhaseAuditSV.Update(objEdit, User.ID);
                        if (!objEdit.IsEdit)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu phê duyệt đợt đánh giá năng lực!", objEdit.Name, objEdit.Approval.ID, User, Common.PhaseAudit, "PhaseAuditID:" + objEdit.ID.ToString());
                        }
                        X.Msg.Notify("Thông báo", "Bạn đã cập nhật thành công!").Show();
                        X.GetCmp<Store>("stMnPhaseAudit").Reload();
                        X.GetCmp<Window>("winEditPhaseAudit").Close();
                        return this.Direct();
                    }
                }
                catch (Exception ex)
                {
                    return this.Direct("Error");
                }
            
            
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                humanPhaseAuditSV.Delete(stringId);
                X.GetCmp<Store>("stMnPhaseAudit").Reload();
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
                    Message = "Đợt đánh giá năng lực đã thực hiện không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = humanPhaseAuditSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }

    }
}
