using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Base;
using iDAS.Items;
using iDAS.Web.Controllers;
using iDAS.Utilities;
using iDAS.DA;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Quản lý công việc tổ chức", IsActive = true, Icon = "Cog", IsShow = true, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TaskController : BaseController
    {

        private TaskSV taskSV = new TaskSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private TaskDA tasksDA = new TaskDA();
        private DepartmentSV departmentSV = new DepartmentSV();
        private TaskCalendarSV calendarsService = new TaskCalendarSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            var data = calendarsService.GetEventAll();
            string style = "";
            foreach (var item in data)
            {
                style += ".gnt-datepicker-" + item.ID + "{background: " + item.Color + "}";
            }
            ViewBag.Style = style;
            ViewBag.Calendar = data;
            return View();
        }
        private Node createNodeTask(TaskItem dataNode, int departmentId)
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
        public ActionResult LoadTreeTasks(string node, int dpId = 0, Nullable<DateTime> fromdate=null, Nullable<DateTime> todate=null, string choise = "")
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
                var tasks = taskSV.GetTreeTask(taskId, dpId, User.Administrator, User.EmployeeID, User.ID, fromdate, todate, choise);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, dpId));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTreeTaskParents(string node, int dpId = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = taskSV.GetTreeTaskParent(taskId, dpId, User.Administrator, User.EmployeeID, User.ID);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, dpId));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTasks(StoreRequestParameters parameters, int dpId = 0)
        {

            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = taskSV.GetAll(filter, dpId);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult ListByServer(string urlSubmit = "", string urlStore = "", StoreParameter paramStore = null)
        {
            ViewData["SubmitUrl"] = urlSubmit;
            ViewData["StoreUrlTask"] = urlStore;
            ViewData["StoreParamTask"] = paramStore == null ? new StoreParameter() : paramStore;
            return PartialView("List");
        }
        public ActionResult List(string containerId = "", string urlSubmit = "", string urlUpdate = "", string urlStore = "", StoreParameter paramStore = null)
        {
            ViewData["SubmitUrl"] = urlSubmit;
            ViewData["UpdateUrl"] = urlUpdate;
            ViewData["StoreUrlTask"] = urlStore;
            ViewData["StoreParamTask"] = paramStore == null ? new StoreParameter() : paramStore;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
                RenderMode = RenderMode.AddTo,
                ContainerId = containerId,
                WrapByScriptTag = false,
            };
        }
        public ActionResult ListPartial(string containerId = "", string urlSubmit = "", string urlUpdate = "", string urlStore = "", StoreParameter paramStore = null)
        {
            ViewData["SubmitUrl"] = urlSubmit;
            ViewData["UpdateUrl"] = urlUpdate;
            ViewData["StoreUrlTask"] = urlStore;
            ViewData["StoreParamTask"] = paramStore == null ? new StoreParameter() : paramStore;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
                RenderMode = RenderMode.AddTo,
                ContainerId = containerId,
                WrapByScriptTag = false,
            };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        // HungNM. Test to add new form for creating task from breakdown module.
        public ActionResult InsertWindow(string urlSubmit = "", Parameter param = null, int departId = 0, int taskId = 0)
        //public ActionResult CreateNewBreakDownTask(string urlSubmit = "", Parameter param = null, int departId = 0, int taskId = 0)
        // End.
        {
            ViewData["UrlSubmit"] = urlSubmit;
            ViewData["Parameter"] = param;
            ViewData["EmployeeID"] = User.EmployeeID;
            TaskItem obj = new TaskItem();
            if (departId != 0)
            {
                obj.Department = new HumanDepartmentViewItem
                {
                    ID = departId,
                    Name = departmentSV.GetByID(departId).Name,

                };

            }
            obj.ParentID = taskId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
                Model = obj,
            };
        }
        public ActionResult Insert(TaskItem objNew, string arrObject = null)
        {
            bool success = false;
            try
            {
                if (objNew.ParentID != 0 && taskSV.GetByID(objNew.ParentID).IsPause)
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
                if (objNew.Perform.ID == 0)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Bạn phải chọn người thực hiện công việc!"
                    });
                    return this.Direct();
                }
                else
                {
                    if (objNew.ID == 0)
                    {
                        if (taskSV.CheckNameTaskExist(objNew.Name.Trim()))
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
                            string nameOld = objNew.Name;
                            if (!string.IsNullOrEmpty(arrObject))
                            {
                                objNew.Perform = employeeSV.GetEmployeeView(objNew.Perform.ID);
                                objNew.Name = nameOld + " (" + objNew.Perform.Name + ")";
                            }
                            int taskId = taskSV.Insert(objNew, User.ID, User.EmployeeID);
                            if (!objNew.IsEdit && !objNew.IsNew)
                            {
                                NotifyController notify = new NotifyController();
                                notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", objNew.Name, objNew.Perform.ID, User, Common.Task, "TaskID:" + taskId.ToString());
                            }
                            if (!string.IsNullOrEmpty(arrObject))
                            {
                                var ids = arrObject.Split(',').Select(n => (object)int.Parse(n)).ToList();
                                ids.Remove(objNew.Perform.ID);
                                foreach (var id in ids)
                                {
                                    objNew.Perform = employeeSV.GetEmployeeView((int)id);
                                    objNew.Name = nameOld + " (" + objNew.Perform.Name + ")";
                                    int taskIds = taskSV.Insert(objNew, User.ID, User.EmployeeID);
                                    if (!objNew.IsEdit && !objNew.IsNew)
                                    {
                                        NotifyController notify = new NotifyController();
                                        notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", objNew.Name, objNew.Perform.ID, User, Common.Task, "TaskID:" + taskIds.ToString());
                                    }
                                }
                            }
                            Ultility.ShowNotification(message: RequestMessage.CreateSuccess);
                        }
                    }
                    else
                    {
                        if (taskSV.CheckNameTaskExistEdit(objNew.ID, objNew.Name.Trim()))
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
                            taskSV.UpdateTask(objNew, User.ID, User.EmployeeID);
                            if (!objNew.IsEdit && !objNew.IsNew)
                            {
                                NotifyController notify = new NotifyController();
                                notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", objNew.Name, objNew.Perform.ID, User, Common.Task, "TaskID:" + objNew.ID.ToString());
                            }
                            Ultility.ShowNotification(message: RequestMessage.UpdateSuccess);
                        }
                    }
                    success = true;
                }
            }
            catch
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.FormPanel(success);
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateWindow(int taskId, string urlUpdate = "", Parameter param = null)
        {
            var model = taskSV.GetByID(taskId);
            ViewData["UrlUpdate"] = urlUpdate;
            ViewData["Parameter"] = param;
            return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData, Model = model };
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailWindow(int taskId)
        {
            var model = taskSV.GetByID(taskId);
            return new Ext.Net.MVC.PartialViewResult { Model = model };
        }
        public ActionResult PauseTaskTrue(int taskId)
        {
            var obj = taskSV.GetByID(taskId);
            if (obj.CreateBy != User.ID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền tạm dừng hoặc khởi động công việc này!"
                });
                return this.Direct();
            }
            else
            {
                taskSV.PauseTask(taskId, User.ID, User.EmployeeID);
                var task = taskSV.GetByID(taskId);
                NotifyController notify = new NotifyController();
                if (!task.IsPause)
                {
                    notify.Notify(this.ModuleCode, "Bạn vừa có một công việc bị tạm dừng", task.Name, task.Perform.ID, User, Common.Task, "TaskID:" + taskId.ToString());
                }
                else
                {
                    notify.Notify(this.ModuleCode, "Bạn vừa có một công việc được khởi động lại", task.Name, task.Perform.ID, User, Common.Task, "TaskID:" + taskId.ToString());
                }
                return this.Direct();
            }
        }
        public ActionResult InsertReport(TaskPerformItem objReport)
        {
            try
            {
                taskSV.Insert(objReport, User.ID, User.EmployeeID);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
        public ActionResult EmployeeWindow()
        {
            return new Ext.Net.MVC.PartialViewResult { };
        }
        public ActionResult LoadTaskPartialView()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "TaskSelectWindow" };
        }
        public ActionResult LoadComboTask()
        {
            var lst = taskSV.GetByCombobox();
            return this.Store(lst);
        }
        public ActionResult FormListObject()
        {
            return View("LstObject");
        }
        public ActionResult ObjectCC()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ChoiceObjectAdd" };
        }
        public ActionResult GetYear()
        {
            List<ComboboxItem> lst = new List<ComboboxItem>();
            lst.Add(new ComboboxItem
            {
                ID = 0,
                Name="Tuần này"
            });
             lst.Add(new ComboboxItem
            {
                ID=1,
                Name="Tháng này"
            });
             for (int i = DateTime.Now.Year; i >= 2012; i--)
            {
                lst.Add(new ComboboxItem
                {
                    ID = i,
                    Name = i == DateTime.Now.Year?"Năm nay": i.ToString()
                });
            }
            return this.Store(lst);
        }
        public ActionResult ShowEmployeeCC(int taskId = 0)
        {
            var obj = taskSV.GetByID(taskId);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "EmployeeCC", Model = obj };
        }
        public ActionResult GetAllEmployee(StoreRequestParameters parameters, string query = "")
        {
            int totalCount = 0;
            var result = new List<HumanEmployeeItem>();
            result = employeeSV.GetAll(parameters.Page, parameters.Limit, out totalCount, query);
            foreach (HumanEmployeeItem emp in result)
            {
                string strChucDanh = "";
                if (emp.IsTrial)
                    strChucDanh = "Thử việc";
                else
                {
                    foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                    {
                        strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                    }
                }
                emp.ChucDanh = strChucDanh;
            }
            return this.Store(result, totalCount);
        }
        public ActionResult LoadEmployeeCC(StoreRequestParameters parameters, int taskId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lsEmployee = taskSV.GetEmployeeCC(filter, taskId);
            return this.Store(lsEmployee, filter.PageTotal);
        }
        public ActionResult AddCC(int taskId = 0, int employeeId = 0)
        {
            taskSV.AddEmployeeCC(taskId, employeeId, User.ID);
            var obj = taskSV.GetByID(taskId);
            NotifyController notify = new NotifyController();
            notify.Notify(this.ModuleCode, "Có một công việc bạn có quyền theo dõi", obj.Name, employeeId, User, Common.TaskCC, "TaskID:" + taskId);
            X.GetCmp<Store>("StoreEmployeeCC").Reload();
            return this.Direct();
        }
        public ActionResult DeleteCC(string stringId)
        {
            try
            {
                taskSV.DeleteCC(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("StoreEmployeeCC").Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã xảy ra lỗi trong quá trình xóa dữ liệu!"
                });
                return this.Direct("Error");
            }
        }
    }
}
