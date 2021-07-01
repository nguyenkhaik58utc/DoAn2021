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
using iDAS.Utilities;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Danh sách công việc cần theo dõi", IsActive = true, Icon = "Cog", IsShow = true, Position = 7)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TaskCCController : BaseController
    {
        //
        // GET: /Task/TaskCC/
        private TaskSV taskSV = new TaskSV();
        private TaskPersonalSV taskPersonalSV = new TaskPersonalSV();
        public ActionResult Index()
        {
            return View();
        }
        private Node createNodeTask(TaskPersonalItem dataNode, int focusId)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.TaskID.ToString();
            nodeItem.Text = !dataNode.Leaf ? dataNode.Name.ToUpper() : dataNode.Name;
            nodeItem.Icon = !dataNode.Leaf ? Icon.Folder : Icon.TagBlue;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TaskID", Value = dataNode.TaskInfo.ID.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.Parent.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Rate", Value = JSON.Serialize(dataNode.TaskInfo.Rate), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsNew", Value = JSON.Serialize(dataNode.TaskInfo.IsNew), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsEdit", Value = JSON.Serialize(dataNode.TaskInfo.IsEdit), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsComplete", Value = JSON.Serialize(dataNode.TaskInfo.IsComplete), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Rate", Value = JSON.Serialize(dataNode.TaskInfo.Rate), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsPause", Value = JSON.Serialize(dataNode.TaskInfo.IsPause), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CategoryName", Value = dataNode.TaskInfo.CategoryName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Status", Value = dataNode.TaskInfo.Status.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelName", Value = dataNode.TaskInfo.LevelName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelColor", Value = dataNode.TaskInfo.LevelColor.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "StartTime", Value = dataNode.TaskInfo.StartTime.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "EndTime", Value = dataNode.TaskInfo.EndTime.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsRequireCheck", Value = JSON.Serialize(dataNode.TaskInfo.IsRequireCheck), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsPerform", Value = JSON.Serialize(dataNode.IsPerform), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsCreate", Value = JSON.Serialize(dataNode.IsCreate), Mode = ParameterMode.Value });
            if (dataNode.TaskID == focusId)
            {
                nodeItem.Cls = "idas-focus";
            }
            else
            {
                if (dataNode.TaskInfo.IsRequireCheck)
                {
                    nodeItem.Cls = "IsRequireCheck";
                }
                else
                {
                    nodeItem.Cls = "";
                }
            }
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        public ActionResult LoadTreeTaskCC(string node, int focusId = 0, int cateId = 0, Nullable<DateTime> fromdate = null, Nullable<DateTime> todate = null, string choise = "")
        {
            try
            {
                if (fromdate == null && todate == null)
                {
                    var date = DateTime.Now.Date;
                    fromdate = new DateTime(date.Year, date.Month, 1);
                    todate = new DateTime(date.Year, 12, 31);
                }
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskPersonalSV.GetTreeTaskCC(taskId, User.EmployeeID, focusId, cateId, fromdate, todate, choise);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, focusId));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult GetYear()
        {
            List<ComboboxItem> lst = new List<ComboboxItem>();
            lst.Add(new ComboboxItem
            {
                ID = 0,
                Name = "Tuần này"
            });
            lst.Add(new ComboboxItem
            {
                ID = 1,
                Name = "Tháng này"
            });
            for (int i = DateTime.Now.Year; i >= 2012; i--)
            {
                lst.Add(new ComboboxItem
                {
                    ID = i,
                    Name = i == DateTime.Now.Year ? "Năm nay" : i.ToString()
                });
            }
            return this.Store(lst);
        }
    }
}
