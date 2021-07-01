using Ext.Net;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Task.Controllers
{

    [SystemAuthorize(CheckAuthorize = false)]
    public class CheckController : BaseController
    {
        //
        // GET: /Task/Checks/
        private TaskCheckSV checksService = new TaskCheckSV();
        private TaskSV taskService = new TaskSV();
        private TaskPerformSV taskPerformSV = new TaskPerformSV();
        private readonly string storeCheck = "stTaskCheck";
        public ActionResult GetData(StoreRequestParameters parameters, int taskId)
        {
            int totalCount;
            var checks = checksService.GetByTaskId(parameters.Page, parameters.Limit, out totalCount, taskId);
            return this.Store(checks, totalCount);
        }
        public ActionResult GetDataPerformUnCheck(StoreRequestParameters parameters, int taskId)
        {
            int totalCount;
            var checks = taskPerformSV.GetByTaskIdUnCheck(parameters.Page, parameters.Limit, out totalCount, taskId);
            return this.Store(checks, totalCount);
        }
        public ActionResult ViewChecks(int taskId)
        {
            bool isHidden = false;
            var obj = taskService.GetByID(taskId);
            if (obj.IsPause)
                isHidden = true;
            if (obj.CreateBy != User.ID)
                isHidden = true;
            if (obj.IsEdit)
                isHidden = true;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Checks", Model= obj, ViewData = new ViewDataDictionary {{ "isHidden", isHidden } } };
        }
        public ActionResult ShowAddChecks(int taskId)
        {
            if (checksService.GetIsFinish(taskId))
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Thông báo",
                    Message = "Công việc đã hoàn thành không được kiểm tra lại!"
                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "AddCheck", Model = taskService.GetCheckByTaskID(taskId), ViewData = new ViewDataDictionary { { "TaskID", taskId } } };
            }
        }
        public ActionResult ShowDetailCheck(int taskId, int checksId)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = checksService.GetByID(taskId, checksId), ViewData = new ViewDataDictionary { { "TaskID", taskId } } };
        }
        public ActionResult Insert(TaskCheckItem objCheck)
        {
            try
            {
                int checkId = checksService.Insert(objCheck, User.ID, User.EmployeeID);
                taskService.UpdateIsPass(objCheck.TaskID, objCheck.Rate, objCheck.IsComplete, objCheck.CompleteTime);
                var task = taskService.GetByID(objCheck.TaskID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một công việc đã được kiểm tra", iDAS.Utilities.Common.SubString(objCheck.Content, 150), task.Perform.ID, User, Common.Task, "TaskID:" + objCheck.TaskID.ToString());
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>(storeCheck).Reload();
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
