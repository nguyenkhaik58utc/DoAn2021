using iDAS.Core;
using iDAS.Items;

using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Utilities;
using iDAS.Web.Controllers;
namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Công văn đến của cá nhân", IsActive = true, Icon = "PackageIn", IsShow = true, Position = 3, Parent = DispatchToMenuController.Code)]
    public class DispatchToEmployeeController : BaseController
    {
        private DispatchToSV dispatchToSV = new DispatchToSV();
        private DispatchUrgencySV dispatchUrgencySV = new DispatchUrgencySV();
        private DispatchSercuritySV dispatchSecuritySV = new DispatchSercuritySV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private TaskSV taskSV = new TaskSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id = 0)
        {
            var obj = dispatchToSV.GetByIDForPerson(id, User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Xác nhận")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmVerify(int id = 0)
        {
            var obj = dispatchToSV.GetVerifyInfoByID(id, User.EmployeeID, true);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Verify", Model = obj };
        }
        public ActionResult LoadDispatchToCboUrgency()
        {
            var lst = dispatchUrgencySV.GetAll();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboSecurity()
        {
            var lst = dispatchSecuritySV.GetAll();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToEmployee(StoreRequestParameters parameters, int focusId = 0)
        {
            int id = User.EmployeeID;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = dispatchToSV.GetAllByUserLogOn(filter, id, focusId);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult VerifyDispatch(DispatchToItem obj, bool isEmployee, string fName, string store = "")
        {
            bool ck = update(obj, isEmployee);
            if (ck)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                if (obj.SendBy != 0)
                {
                    NotifyController notify = new NotifyController();
                    notify.NotifyUser(this.ModuleCode, "Có một công văn đã xác nhận!", obj.Name, obj.SendBy, User, Common.DispatchToDepartment, "DispatchID:" + obj.ID.ToString());
                }
                if (!store.Equals(""))
                    X.GetCmp<Store>(store).Reload();
                X.GetCmp<Window>(fName).Close();
            }
            else
                Ultility.CreateNotification(message: RequestMessage.UpdateError);
            return this.Direct();
        }
        private bool update(DispatchToItem obj, bool isEmployee = false)
        {
            obj.UpdateBy = User.ID;
            dispatchToSV.UpdateVerify(obj, isEmployee);
            return true;
        }
        private Node createNodeTask(TaskItem dataNode, int dispatchID)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            //if (dataNode. != planId)
            //{
            //    nodeItem.Cls = "clsUnView";
            //}
            nodeItem.Text = !dataNode.Leaf ? dataNode.Name.ToUpper() : dataNode.Name;
            nodeItem.Icon = !dataNode.Leaf ? Icon.Folder : Icon.TagBlue;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Rate", Value = JSON.Serialize(dataNode.Rate), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsNew", Value = JSON.Serialize(dataNode.IsNew), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsEdit", Value = JSON.Serialize(dataNode.IsEdit), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsComplete", Value = JSON.Serialize(dataNode.IsComplete), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsPause", Value = JSON.Serialize(dataNode.IsPause), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsPause", Value = JSON.Serialize(dataNode.IsPause), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CategoryName", Value = dataNode.CategoryName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Status", Value = dataNode.Status.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelName", Value = dataNode.LevelName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelColor", Value = dataNode.LevelColor.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "StartTime", Value = dataNode.StartTime.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "EndTime", Value = dataNode.EndTime.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        public ActionResult LoadTasks(string node, int dispatchID = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = dispatchToSV.GetTreeTask(taskId, dispatchID);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, dispatchID));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult InsertTask(TaskItem task, int dispatchID = 0, string arrObject = null)
        {
            bool success = false;
            try
            {
                if (task.ParentID != 0 && taskSV.GetByID(task.ParentID).IsPause)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Công việc đã tạm dừng không được phép thêm công việc con!"
                    });
                    return this.Direct();
                }
                if (task.ID == 0)
                {
                    if (taskSV.CheckNameTaskExist(task.Name.Trim()))
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.WARNING,
                            Title = "Thông báo",
                            Message = "Tên công việc đã tồn tại vui lòng nhập tên khác!"
                        });
                        return this.Direct();
                    }
                    else
                    {
                        string nameOld = task.Name;
                        if (!string.IsNullOrEmpty(arrObject))
                        {
                            task.Perform = employeeSV.GetEmployeeView(task.Perform.ID);
                            task.Name = nameOld + " (" + task.Perform.Name + ")";
                        }
                        task.CreateBy = User.ID;
                        int taskId = taskSV.Insert(task, User.ID, User.EmployeeID);
                        dispatchToSV.InsertTask(taskId, dispatchID, User.ID, task.Content);
                        if (!task.IsEdit && !task.IsNew)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "DispatchTaskID:" + taskId.ToString());
                        }
                        if (!string.IsNullOrEmpty(arrObject))
                        {
                            var ids = arrObject.Split(',').Select(n => (object)int.Parse(n)).ToList();
                            ids.Remove(task.Perform.ID);
                            foreach (var ide in ids)
                            {
                                task.Perform = employeeSV.GetEmployeeView((int)ide);
                                task.Name = nameOld + " (" + task.Perform.Name + ")";
                                int taskIds = taskSV.Insert(task, User.ID, User.EmployeeID);
                                if (!task.IsEdit && !task.IsNew)
                                {
                                    NotifyController notify = new NotifyController();
                                    notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "DispatchTaskID:" + taskIds.ToString());
                                }
                                dispatchToSV.InsertTask(taskIds, dispatchID, User.ID, task.Content);
                            }
                        }
                        Ultility.ShowNotification(message: RequestMessage.CreateSuccess);
                        success = true;
                    }
                }
                else
                {
                    if (taskSV.CheckNameTaskExistEdit(task.ID, task.Name.Trim()))
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.WARNING,
                            Title = "Thông báo",
                            Message = "Tên công việc đã tồn tại vui lòng nhập tên khác!"
                        });
                        return this.Direct();
                    }
                    else
                    {
                        taskSV.UpdateTask(task, User.ID, User.EmployeeID);
                        Ultility.ShowNotification(message: RequestMessage.UpdateSuccess);
                        success = true;
                    }
                }
            }
            catch
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.FormPanel(success);
        }
    }
}
