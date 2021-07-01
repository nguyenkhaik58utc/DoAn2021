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
    [BaseSystem(Name = "Công văn đến trong nội bộ", Icon = "PackageIn", IsActive = true, IsShow = true, Position = 2, Parent = DispatchToMenuController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DispatchToInSideController : BaseController
    {
        private DispatchGoSV dispatchGoSV = new DispatchGoSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private TaskSV taskSV = new TaskSV();
        private DispatchCategorySV dispatchCateSV = new DispatchCategorySV();
        private DispatchMethodService dispatchMethodSV = new DispatchMethodService();
        private DispatchSercuritySV dispatchSecuritySV = new DispatchSercuritySV();
        private DispatchUrgencySV dispatchUrgencySV = new DispatchUrgencySV();
        private HumanEmployeeSV employeeService = new HumanEmployeeSV();
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
            var obj = dispatchGoSV.GetVerifyInfoByID(id, User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Xác nhận")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmVerify(int id = 0)
        {
            var obj = dispatchGoSV.GetVerifyInfoByID(id, User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Verify", Model = obj };
        }
        public ActionResult showFrmVerifyIn(int id = 0, int type = 0, bool fromType = true)
        {
            if (id > 0)
            {
                if (type == (int)DispatchObjectType.Department)
                {
                    var obj = dispatchGoSV.GetObjectVerifyByID(id, User.EmployeeID);
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "VerifyInDetail", Model = obj, ViewData = new ViewDataDictionary { { "fromType", fromType } } };
                }
                else if (type == (int)DispatchObjectType.Employee)
                {
                    var obj = dispatchGoSV.GetObjectVerifyByID(id, User.EmployeeID, true);
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "VerifyInDetail", Model = obj, ViewData = new ViewDataDictionary { { "fromType", fromType } } };
                }
            }
            return null; ;
        }
        public ActionResult ShowFrmVerifyInDetail(int id = 0, int type = 0)
        {
            if (id > 0)
            {
                if (type == (int)DispatchObjectType.Department)
                {
                    var obj = dispatchGoSV.GetObjectVerifyByID(id, User.EmployeeID);
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "VerifyInDetail", Model = obj };
                }
                else if (type == (int)DispatchObjectType.Employee)
                {
                    var obj = dispatchGoSV.GetObjectVerifyByID(id, User.EmployeeID, true);
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "VerifyInDetail", Model = obj };
                }
            }
            return null; ;
        }
        public ActionResult ShowFrmVerifyDetail(int id = 0, int type = 0)
        {
            if (id > 0)
            {
                if (type == (int)DispatchObjectType.Department)
                {
                    var obj = dispatchGoSV.GetObjectVerifyByID(id, User.EmployeeID);
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailVerify", Model = obj };
                }
                else if (type == (int)DispatchObjectType.Employee)
                {
                    var obj = dispatchGoSV.GetObjectVerifyByID(id, User.EmployeeID, true);
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailVerify", Model = obj };
                }
            }
            return null; ;
        }
        public ActionResult UpdateVerifyInDetail(DispatchGoObjectItem obj, bool fromType = true)
        {
            bool isEmployee = true;
            if (obj.Type == (int)DispatchObjectType.Department)
                isEmployee = false;
            dispatchGoSV.UpdateVerify(obj, User.ID, isEmployee);
            if (obj.SendBy != 0)
            {
                NotifyController notify = new NotifyController();
                notify.NotifyUser(this.ModuleCode, "Có một công văn đã xác nhận!", obj.Name, obj.SendBy, User, fromType ? Common.DispatchGoDepartment : Common.DispatchGoEmployee, "DispatchID:" + obj.DispatchGo.ToString());
            }
            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            X.GetCmp<Store>("stObjectVerify").Reload();
            X.GetCmp<Window>("winVerifyInDetail").Close();
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
                        dispatchGoSV.InsertTask(taskId, dispatchID, User.ID, task.Content);
                        if (!task.IsEdit && !task.IsNew)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "DiskpatchTaskID:" + taskId.ToString());
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
                                    notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", task.Name, task.Perform.ID, User, Common.Task, "DiskpatchTaskID:" + taskIds.ToString());
                                }
                                dispatchGoSV.InsertTask(taskIds, dispatchID, User.ID, task.Content);
                            }
                        }

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
        public ActionResult LoadDispatchGo(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<int> us = User.GroupIDs;
            var lst = dispatchGoSV.GetAllDispatchInSide(filter, us, User.EmployeeID, focusId);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult GetDataMoveIn(int id = 0)
        {
            List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
            if (id > 0)
            {
                lst = dispatchGoSV.GetAllObjectVerifyInByID(id, User.GroupIDs, User.EmployeeID);
            }
            return this.Store(lst);
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
                var tasks = dispatchGoSV.GetTreeTask(taskId, dispatchID);
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
        private bool chekValid(DispatchGoItem obj)
        {
            return true;
        }
        public ActionResult ShowEmployeeCC(int dispatchGoId = 0)
        {
            var obj = dispatchGoSV.GetByID(dispatchGoId);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "EmployeeCC", Model = obj };
        }
        public ActionResult GetAllEmployee(StoreRequestParameters parameters, string query = "")
        {
            int totalCount = 0;
            var result = new List<HumanEmployeeItem>();
            result = employeeService.GetAll(parameters.Page, parameters.Limit, out totalCount, query);
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
        public ActionResult LoadEmployeeCC(StoreRequestParameters parameters, int dispatchGoId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lsEmployee = dispatchGoSV.GetEmployeeCC(filter, dispatchGoId);
            return this.Store(lsEmployee, filter.PageTotal);
        }
        public ActionResult AddCC(int dispatchGoId = 0, int employeeId = 0)
        {
            dispatchGoSV.AddEmployeeCC(dispatchGoId, employeeId, User.ID);
            var obj = dispatchGoSV.GetByID(dispatchGoId);
            NotifyController notify = new NotifyController();
            notify.Notify(this.ModuleCode, "Có một công văn đi bạn có quyền theo dõi", obj.Name, employeeId, User, Common.DispatchGoCC, "DispatchID:" + dispatchGoId);
            X.GetCmp<Store>("StoreEmployeeCC").Reload();
            return this.Direct();
        }
        public ActionResult DeleteCC(string stringId)
        {
            try
            {
                dispatchGoSV.DeleteCC(stringId);
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
