using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Thống kê công việc theo phòng ban", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 2, Parent = GroupStatisticalMenuController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DepartmentStatisticalTaskController : BaseController
    {
        //
        // GET: /Task/DepartmentStatisticalTask/
        private TaskSV taskSV = new TaskSV();
        private DepartmentSV departmentSV = new DepartmentSV();
        [BaseSystem(Name = "Xem dữ liệu thống kê", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(string node, string fromDate, string toDate)
        {
            NodeCollection nodes = new NodeCollection();
            int id = node == "root" ? 0 : System.Convert.ToInt32(node);
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            else
            {
                searchFromDate = DateTime.Now;
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            else
            {
                searchToDate = DateTime.Now;
            }
            var lsDataNodes = new ChartModel().GetDepartmentStatisticalTask(id, searchFromDate, searchToDate);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.DepartmentID.ToString();
                nodeItem.Text = dataNode.DepartmentName;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "DepartmentName", Value = dataNode.DepartmentName.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalTask", Value = dataNode.TotalTask.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalPause", Value = dataNode.TotalPause.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalOverTime", Value = dataNode.TotalOverTime.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalFinish", Value = dataNode.TotalFinish.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDoing", Value = dataNode.TotalDoing.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalFinishOverTime", Value = dataNode.TotalFinishOverTime.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalAuditNot", Value = dataNode.TotalAuditNot.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalAuditGood", Value = dataNode.TotalAuditGood.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalAuditFail", Value = dataNode.TotalAuditFail.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }
        public StoreResult GetDataPie(int id, string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateData(id, searchFromDate, searchToDate));
        }
        public StoreResult GetDataColumn(int id, string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateDataColumn(id, searchFromDate, searchToDate));
        }
        /// <summary>
        /// Audit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public StoreResult GetDataAuditPie(int id, string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateAuditData(id, searchFromDate, searchToDate));
        }
        public StoreResult GetDataAuditColumn(int id, string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateDataAuditColumn(id, searchFromDate, searchToDate));
        }
        public ActionResult TaskViewStatistical(string urlStore = "", StoreParameter param = null, string fromDate = "", string toDate = "")
        {
            ViewData["StoreUrlTask"] = urlStore;
            ViewData["StoreParamTaskStatiscal"] = param;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        private Node createNodeTask(TaskItem dataNode, int departmentId, DateTime fromDate, DateTime toDate)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            if (dataNode.DepartmentID != departmentId)
            {
                nodeItem.Cls = "clsUnView";
            }
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
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalTime", Value = dataNode.TotalTime.ToString(), Mode = ParameterMode.Value });
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        /// <summary>
        /// View task statistical
        /// </summary>
        /// <param name="node"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ActionResult LoadTotalTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeTotalTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeTotalTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadPauseTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreePauseTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreePauseTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadOverTimeTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeOverTimeTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeOverTimeTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadDoingTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeDoingTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeDoingTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadFinishTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeFinishTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeFinishTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadFinishOverTimeTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeFinishOverTimeTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeFinishOverTimeTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        /// <summary>
        /// Xem công việc đánh giá
        /// </summary>
        /// <param name="node"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ActionResult LoadAuditNotTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeAuditNotTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeAuditNotTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadAuditGoodTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeAuditGoodTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeAuditGoodTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadAuditFailTasks(string node, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = new List<TaskItem>();
                if (departmentSV.GetByID(departmentId).ParentID != 0)
                    tasks = taskSV.GetTreeAuditFailTaskByDepartmentID(taskId, departmentId, searchFromDate, searchToDate);
                else
                    tasks = taskSV.GetTreeAuditFailTaskAll(taskId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, departmentId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
    }
}
