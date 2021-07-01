using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Services;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Risk.Controllers
{
    public class RiskControlTaskController : BaseController
    {                                          
        //
        // GET: /Risk/RiskControlTask/
        private RiskControlTaskSV RiskControlTaskSV = new RiskControlTaskSV();
        private TaskSV taskSV = new TaskSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private Node createNodeTask(TaskItem dataNode, int planId)
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
        public ActionResult LoadTasks(string node, int RiskControlID = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = RiskControlTaskSV.GetTreeTask(taskId, RiskControlID);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, RiskControlID));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult InsertTask(TaskItem task, int RiskControlID, string arrObject = null)
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
                        int taskId = RiskControlTaskSV.InsertRiskControl(task, RiskControlID, User.ID, User.EmployeeID);
                        if (!task.IsEdit && !task.IsNew)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "RiskControlTaskID:" + taskId.ToString());
                        }
                        if (!string.IsNullOrEmpty(arrObject))
                        {
                            var ids = arrObject.Split(',').Select(n => (object)int.Parse(n)).ToList();
                            ids.Remove(task.Perform.ID);
                            foreach (var ide in ids)
                            {
                                task.CreateBy = User.ID;
                                task.Perform = employeeSV.GetEmployeeView((int)ide);
                                task.Name = nameOld + " (" + task.Perform.Name + ")";
                                int taskIds = RiskControlTaskSV.InsertRiskControl(task, RiskControlID, User.ID, User.EmployeeID);
                                if (!task.IsEdit && !task.IsNew)
                                {
                                    NotifyController notify = new NotifyController();
                                    notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "RiskControlTaskID:" + taskIds.ToString());
                                }

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
