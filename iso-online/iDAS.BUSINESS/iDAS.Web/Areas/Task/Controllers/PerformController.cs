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
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Task.Controllers
{

    [SystemAuthorize(CheckAuthorize = false)]
    public class PerformController : BaseController
    {
        //
        // GET: /Task/Performs/
        private TaskPerformSV performSV = new TaskPerformSV();
        private TaskSV taskSV = new TaskSV();
        private readonly string storePerform = "stTaskPerform";
        public ActionResult GetData(StoreRequestParameters parameters, int taskId)
        {
            int totalCount;
            var checks = performSV.GetByTaskId(parameters.Page, parameters.Limit, out totalCount, taskId);
            return this.Store(checks, totalCount);
        }
        public ActionResult ViewPerforms(int taskId)
        {
            bool isHidden = false;
            var obj = taskSV.GetByID(taskId);
            if (obj.IsPause)
                isHidden = true;
            if (obj.PerformBy != User.EmployeeID)
                isHidden = true;
            if (obj.IsEdit)
                isHidden = true;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Performs", Model = obj, ViewData = new ViewDataDictionary { { "isHidden", isHidden } } };
        }
        public ActionResult ShowAddPerform(int taskId)
        {
            if (taskSV.GetByID(taskId).IsComplete)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Thông báo",
                    Message = "Công việc đã hoàn thành không được phép báo cáo lại!"
                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "AddPerform", Model = taskSV.GetByTaskID(taskId), ViewData = new ViewDataDictionary { { "TaskID", taskId } } };
            }
        }
        public ActionResult ShowDetailPerform(int taskId, int performsId)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = performSV.GetByID(taskId, performsId), ViewData = new ViewDataDictionary { { "TaskID", taskId } } };
        }
        public ActionResult Insert(TaskPerformItem objPerforms)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    performSV.Insert(objPerforms, User.ID, User.EmployeeID);
                    var task = taskSV.GetByID(objPerforms.TaskID);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một báo cáo công việc yêu cầu kiểm tra", iDAS.Utilities.Common.SubString(objPerforms.Content, 150), task.Create.ID, User, Common.Task, "TaskID:" + objPerforms.TaskID.ToString());
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>(storePerform).Reload();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
    }
}
