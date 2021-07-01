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
using iDAS.Items;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Thống kê công việc theo nhóm", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 1, Parent = GroupStatisticalMenuController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CateStatisticalTaskController : BaseController
    {
        //
        // GET: /Task/CateStatisticalTask/
        private TaskSV taskSV = new TaskSV();
        [BaseSystem(Name = "Xem dữ liệu thống kê", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, string fromDate, string toDate)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = new List<iDAS.Services.ChartModel.CateStatisticalInfo>();
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
            result = ChartModel.GetCateStatisticalTask(filter, searchFromDate, searchToDate);
            return this.Store(result, filter.PageTotal);
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
            return new StoreResult(ChartModel.GenerateDataCate(id, searchFromDate, searchToDate));
        }
        public StoreResult GetDataColumn(int id, string fromDate, string toDate)
        {
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateDataCateColumn(id, searchStartTime, searchEndTime));
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
            return new StoreResult(ChartModel.GenerateDataAuditCate(id, searchFromDate, searchToDate));
        }
        public StoreResult GetDataAuditColumn(int id, string fromDate, string toDate)
        {
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(ChartModel.GenerateDataAuditCateColumn(id, searchStartTime, searchEndTime));
        }
        /// <summary>
        /// View Task Statistical
        /// </summary>
        /// <param name="dataNode"></param>
        /// <param name="employeeId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private Node createNodeTask(TaskItem dataNode, int employeeId, DateTime fromDate, DateTime toDate)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            if (dataNode.DepartmentID != employeeId)
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
        public ActionResult LoadTotalTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeTotalTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadPauseTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreePauseTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadOverTimeTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeOverTimeTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadDoingTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeDoingTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadFinishTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeFinishTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadFinishOverTimeTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeFinishOverTimeTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
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
        /// View task Audit
        /// </summary>
        /// <param name="node"></param>
        /// <param name="cateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ActionResult LoadAuditNotTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeAuditNotTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadAuditGoodTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeAuditGoodTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadAuditFailTasks(string node, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeAuditFailTaskByCateID(taskId, cateId, searchFromDate, searchToDate);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, cateId, searchFromDate, searchToDate));
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
