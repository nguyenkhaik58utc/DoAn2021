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


namespace iDAS.Web.Areas.Service.Controllers
{

    [BaseSystem(Name = "Thực hiện", Icon = "RubyGo", IsActive = true, IsShow = true, Position = 5)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class PerformPlanController : BaseController
    {
        //
        // GET: /Service/PerformPlan/
        private QualityPlanTaskSV planTaskSV = new QualityPlanTaskSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
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
        public ActionResult LoadTasks(string node, int planId = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = planTaskSV.GetTreeTask(taskId, planId);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, planId));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        //public ActionResult LoadTasks(StoreRequestParameters parameters, int planId = 0)
        //{
        //    ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
        //    var lst = planTaskSV.GetAll(filter, planId);
        //    return this.Store(lst, filter.PageTotal);

        //}
        public ActionResult PlanPerform(int contractId = 0)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "PlanByContract", ViewData = new ViewDataDictionary { { "contractId", contractId } } };
        }
        public ActionResult Insert(TaskItem task, int planId, string arrObject = null)
        {
            bool success = false;
            try
            {
                string nameOld = task.Name;
                if (!string.IsNullOrEmpty(arrObject))
                {
                    task.Perform = employeeSV.GetEmployeeView(task.Perform.ID);
                    task.Name = nameOld + " (" + task.Perform.Name + ")";
                }
                task.CreateBy = User.ID;
                int taskId =  planTaskSV.InsertQualityPlanTask(task, planId, User.ID, User.EmployeeID);
                if (!task.IsEdit && !task.IsNew)
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "PerfomPlanTaskID:" + taskId.ToString());
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
                        int taskIds = planTaskSV.InsertQualityPlanTask(task, planId, User.ID, User.EmployeeID);
                        if (!task.IsEdit && !task.IsNew)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "PerfomPlanTaskID:" + taskIds.ToString());
                        }

                    }
                }
                Ultility.ShowNotification(message: RequestMessage.CreateSuccess);
                success = true;
            }
            catch
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.FormPanel(success);
        }
        public ActionResult GetYear()
        {
            List<ComboboxItem> lst = new List<ComboboxItem>();

            for (int i = 2009; i <= DateTime.Now.Year; i++)
            {
                lst.Add(new ComboboxItem
                {
                    ID = i,
                    Name = i.ToString()
                });
            }
            return this.Store(lst);
        }

    }
}
