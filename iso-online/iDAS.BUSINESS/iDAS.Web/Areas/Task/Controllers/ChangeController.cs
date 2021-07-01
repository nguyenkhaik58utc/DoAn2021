using Ext.Net;
using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Base;
using iDAS.DA;
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Điều chỉnh công việc", IsActive = true, IsShow = false)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ChangeController : BaseController
    {
        //
        // GET: /Task/Changes/
        private TaskHistorySV changesService = new TaskHistorySV();
        private TaskSV taskSV = new TaskSV();
        private readonly string storeChange = "stTaskChange";
        public ActionResult GetData(StoreRequestParameters parameters, int taskId)
        {
            int totalCount;
            var changes = changesService.GetAll(parameters.Page, parameters.Limit, out totalCount, taskId);
            return this.Store(changes, totalCount);
        }
        [BaseSystem(Name = "Xem danh sách điều chỉnh", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ChangeVoteList(int taskId)
        {
            bool isHidden = false;
            bool isHiddenApp = false;
            var obj = taskSV.GetByID(taskId);
            if (obj.IsPause)
                isHidden = true;
            if (!obj.IsEdit && !obj.IsNew && !obj.IsComplete && changesService.GetTaskByID(taskId).TaskHistories.FirstOrDefault(t => t.IsChange == false) == null)
            {
                isHidden = false;
            }
            else
            {
                isHidden = true;
            }
            if (changesService.GetTaskByID(taskId).TaskHistories.FirstOrDefault(t => t.IsChange == false) != null)
            {
                isHidden = true;
                isHiddenApp = false;
            }
            if (obj.Create.ID!=User.EmployeeID && obj.Perform.ID != User.EmployeeID)
            {
                isHidden = true;
                isHiddenApp = true;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChangeVoteList", ViewData = new ViewDataDictionary { { "TaskID", taskId }, { "isHidden", isHidden }, { "isHiddenApp", isHiddenApp } } };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddChangeVote(int taskId)
        {
            if (taskSV.GetByID(taskId).IsComplete)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Thông báo",
                    Message = "Công việc đã hoàn thành không được phép điều chỉnh công việc!"
                });
                return this.Direct();
            }
            else
            {
                if (changesService.GetTaskByID(taskId).TaskHistories.FirstOrDefault(t => t.IsChange == false) != null)
                {

                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO,
                        Title = "Thông báo",
                        Message = "Bạn chỉ được phép sửa phiếu điều chỉnh chưa được duyệt!"
                    });
                    return this.Direct();
                }
                else
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "AddChangeVote", Model = taskSV.GetByID(taskId) };
                }
            }
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult EditChangeVote(int taskId)
        {
            var rs = changesService.GetTaskByID(taskId).TaskHistories.FirstOrDefault(m => m.IsChange == false);
            if (rs != null)
            {
               return new Ext.Net.MVC.PartialViewResult { ViewName = "EditChangeVote", Model = changesService.GetByID(rs.ID) };
            }
            else
            {
                Ultility.CreateMessageBox(title: "Thông báo", message: "Không có phiếu điều chỉnh nào chưa duyệt!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ViewChangeDetail(int changeId)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = changesService.GetByID(changeId) };
        }
        [BaseSystem(Name = "Phê duyệt", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ApproveChangeVote(int taskId)
        {
            if (taskSV.GetByID(taskId).IsComplete)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Thông báo",
                    Message = "Công việc đã hoàn thành không được phép điều chỉnh công việc!"
                });
                return this.Direct();
            }
            else
            {
                var rs = changesService.GetTaskByID(taskId).TaskHistories.FirstOrDefault(m => m.IsChange == false);
                if (rs == null)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Không có phiếu điều chỉnh nào cần phê duyệt!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct();
                }
                else if (rs.Task.CreateBy != User.ID)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn không có quyền xác nhận phiếu điều chỉnh này!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct();
                }
                else
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "ApproveChange", Model = changesService.GetByID(rs.ID) };
                }
            }
        }
        public ActionResult SaveApprove(TaskHistoryItem obj)
        {
            try
            {
                TaskHistory change = new TaskHistory();
                change = changesService.GetById(obj.ID);
                var task = changesService.GetTaskByID(obj.TaskID);
                if (obj.IsChange)
                {
                    taskSV.UpdateChange(change);
                    changesService.Update(task, obj.ID, obj.ChangeNote);
                    TaskItem taskchange = new TaskItem();
                    taskchange.ID = change.TaskID;
                    taskchange.CategoryID = change.CategoryID;
                    taskchange.Cost = change.Cost;
                    taskchange.PerformBy = change.EmployeeID;
                    changesService.UpdateTaskPerson(taskchange, User.EmployeeID, User.ID);
                }
                else
                {
                    changesService.UpdateNoteChange(obj, User.ID);
                }
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Bạn có một công việc được điều chỉnh", iDAS.Utilities.Common.SubString(task.Name, 150), task.HumanEmployeeID, User, Common.Task, "TaskID:" + obj.TaskID.ToString());
                X.GetCmp<Store>(storeChange).Reload();
                if (obj.IsChange)
                {
                    return this.Direct(true);
                }
                else {
                    return this.Direct(false);
                }
            }
            catch (Exception)
            {
                return this.Direct();
            }
        }
        public ActionResult UpdateChange(TaskHistoryItem objNew)
        {
            try
            {
                var obj = taskSV.GetByID(objNew.TaskID);
                changesService.UpdateChange(objNew, User.ID, User.EmployeeID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Bạn có một công việc yêu cầu điều chỉnh", obj.Name, obj.Create.ID, User, Common.Task, "TaskID:" + obj.ID.ToString());
                X.GetCmp<Store>(storeChange).Reload();
                X.GetCmp<Store>(storeChange).Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        public ActionResult Insert(TaskItem objNew)
        {
            try
            {
                var obj = taskSV.GetByID(objNew.ID);
                changesService.Insert(objNew, User.ID, User.EmployeeID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Bạn có một công việc yêu cầu điều chỉnh", obj.Name, obj.Create.ID, User, Common.Task, "TaskID:" + obj.ID.ToString());
                X.GetCmp<Store>(storeChange).Reload();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
            }
            return this.Direct();
        }
    }
}
