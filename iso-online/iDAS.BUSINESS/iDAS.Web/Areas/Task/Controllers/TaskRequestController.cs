using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;
using iDAS.Items;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Đề xuất thực hiện công việc", IsActive = true, Icon = "Cog", IsShow = true, Position = 6)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TaskRequestController : BaseController
    {
        private TaskRequestSV taskRequestSV = new TaskRequestSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        //
        // GET: /Task/TaskRequest/
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.EmployeeID = User.EmployeeID;
            ViewBag.EmployeeName = User.Name;
            ViewBag.FocusId = focusId;
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0, int employeeId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = taskRequestSV.GetAll(filter, focusId, employeeId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult ShowFrmAdd()
        {
            TaskRequestItem obj = new TaskRequestItem();
            obj.Perform = employeeSV.GetEmployeeView(User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create", Model = obj };
        }
        public ActionResult ShowUpdate(int id)
        {
            var obj = taskRequestSV.GetById(id);
            if (obj.CreateBy != User.ID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền sửa đề xuất này!"
                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Update", Model = obj };
            }
        }
        public ActionResult ShowApprove(int id)
        {
            var obj = taskRequestSV.GetById(id);
            if (obj.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt đề xuất thực hiện công việc này!"
                });
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Approve", Model = obj };
            }
            return this.Direct();
        }
        public ActionResult SendApprove(TaskRequestItem obj)
        {
            try
            {
                taskRequestSV.SendApprove(obj, User.ID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một đề xuất thực hiện công việc yêu cầu phê duyệt", obj.Name, obj.Approval.ID, User, Common.TaskRequest, "TaskRequestID:" + obj.ID.ToString());
                X.GetCmp<Store>("stTaskRequest").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);

            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        public ActionResult ApproveRequest(TaskRequestItem obj)
        {
            try
            {
                taskRequestSV.Approve(obj, User.ID);
                if (obj.CreateBy.HasValue)
                {
                    NotifyController notify = new NotifyController();
                    notify.NotifyUser(this.ModuleCode, "Có một đề xuất thực hiện công việc đã được phê duyệt", obj.Name, obj.CreateBy.Value, User, Common.TaskRequest, "TaskRequestID:" + obj.ID.ToString());
                }
                if (obj.IsAccept)
                {
                    new TaskSV().Insert(obj, User.ID, User.EmployeeID);
                    NotifyController notify = new NotifyController();
                    notify.NotifyUser(this.ModuleCode, "Bạn có một công việc yêu cầu thực hiện", obj.Name, obj.HumanEmployeeID, User, Common.Task, "TaskID:" + obj.ID.ToString());
                }
                X.GetCmp<Store>("stTaskRequest").Reload();
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
            var obj = taskRequestSV.GetById(id);
            if (obj.Perform.ID != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền gửi phê duyệt đề xuất này!"
                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "SendApproval", Model = obj };
            }
        }
        public ActionResult Insert(TaskRequestItem objNew)
        {
            try
            {
                if (!objNew.Name.Trim().Equals("") && !taskRequestSV.CheckCodeExits(objNew.Name.Trim()))
                {

                    if (objNew.Approval == null || objNew.Approval.ID == 0)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt đề xuất thực hiện công việc!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                    else
                    {
                        objNew.CreateBy = User.ID;
                        taskRequestSV.Insert(objNew, User.ID);
                        X.GetCmp<Store>("stTaskRequest").Reload();
                        X.GetCmp<Window>("wdAddTaskRequest").Close();
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
                        Message = "Tên công việc đã tồn tại vui lòng nhập tên khác!"
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
        public ActionResult Update(TaskRequestItem objEdit)
        {
            try
            {
                if (!objEdit.Name.Trim().Equals("") && taskRequestSV.CheckCodeExitEdits(objEdit.ID, objEdit.Name.Trim()))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Tên công việc đã tồn tại vui lòng nhập tên khác!"
                    });
                }
                else
                {
                    if (objEdit.Approval == null || objEdit.Approval.ID == 0)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt đề xuất thực hiện công việc!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                    else
                    {
                        taskRequestSV.Update(objEdit, User.ID);
                        X.GetCmp<Store>("stTaskRequest").Reload();
                        X.GetCmp<Window>("wdEditTaskRequest").Close();
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
        public ActionResult Delete(string stringId)
        {
            try
            {
                taskRequestSV.Delete(stringId);
                X.GetCmp<Store>("stTaskRequest").Reload();
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
                    Message = "Công việc đã được thực hiện không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        public ActionResult ShowDetail(int id)
        {
            var obj = taskRequestSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
    }
}