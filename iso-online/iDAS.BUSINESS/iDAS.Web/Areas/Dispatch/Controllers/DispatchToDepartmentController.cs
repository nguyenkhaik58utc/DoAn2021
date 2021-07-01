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
    [BaseSystem(Name = "Công văn đến của tổ chức", IsActive = true, Icon = "PackageIn", IsShow = true, Position = 1, Parent = DispatchToMenuController.Code)]
    public class DispatchToDepartmentController : BaseController
    {
        //
        private DispatchToSV dispatchToSV = new DispatchToSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private DispatchCategorySV dispatchCateSV = new DispatchCategorySV();
        private TaskSV taskSV = new TaskSV();
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
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id = 0)
        {
            var obj = dispatchToSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                dispatchToSV.Delete(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("StDispatchToDepartment").Reload();
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
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id = 0)
        {
            var obj = dispatchToSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Chuyển nội bộ")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmMove(int id = 0)
        {
            var obj = dispatchToSV.GetByID(id);
            int idUserLogOn = User != null ? User.ID : 0;
            if (obj != null && obj.CreateBy == idUserLogOn)
                obj.FlagRequired = false;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Move", Model = obj };
        }
        [BaseSystem(Name = "Xác nhận")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmVerify(int id = 0)
        {
            var depiS = User.GroupIDs;
            var obj = dispatchToSV.GetVerifyInfoByID(id, User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Verify", Model = obj };
        }
        public ActionResult ShowFrmDetailVerify(int id = 0, int type = 0)
        {
            if (id > 0)
            {
                var depIDs = User.GroupIDs;
                var obj = dispatchToSV.GetObjectVerifyByID(id, type);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailVerify", Model = obj };
            }
            return null;

        }
        public ActionResult LoadDispatchTo(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = dispatchToSV.GetAll(filter, User.GroupIDs, focusId);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult LoadDispatchToCboCate()
        {
            var lst = dispatchCateSV.GetAll();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboMethod()
        {
            var lst = dispatchMethodSV.GetAll();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboSecurity()
        {
            var lst = dispatchSecuritySV.GetAll();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboUrgency()
        {
            var lst = dispatchUrgencySV.GetAll();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboCateIsActive()
        {
            var lst = dispatchCateSV.GetAllIsActive();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboMethodIsActive()
        {
            var lst = dispatchMethodSV.GetAllIsActive();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboSecurityIsActive()
        {
            var lst = dispatchSecuritySV.GetAllIsActive();
            return this.Store(lst);
        }
        public ActionResult LoadDispatchToCboUrgencyIsActive()
        {
            var lst = dispatchUrgencySV.GetAllIsActive();
            return this.Store(lst);
        }
        //Load dữ liệu cho phòng ban nhận cv
        public ActionResult GetDataDepartment(int id = 0)
        {
            List<DispatchToObjectItem> lst = new List<DispatchToObjectItem>();
            if (id > 0)
            {
                lst = dispatchToSV.GetDepartmentByID(id);
            }
            return this.Store(lst);
        }
        public ActionResult GetDataEmployee(int id = 0)
        {
            List<DispatchToObjectItem> lst = new List<DispatchToObjectItem>();
            if (id > 0)
            {
                lst = dispatchToSV.GetEmployeeByID(id);
            }
            return this.Store(lst);
        }
        public ActionResult GetDataCboDeptByUserLogon(int id)
        {
            List<DispatchToObjectItem> lst = new List<DispatchToObjectItem>();
            if (id > 0)
            {
                lst = dispatchToSV.GetDeptVerifyByID(id, User.GroupIDs);
            }
            return this.Store(lst);
        }
        //lấy toàn bộ thông tin Chuyển - nhận theo ID Công văn đến 
        public ActionResult GetDataObjectVerify(StoreRequestParameters parameters, int id = 0)
        {
            List<DispatchToObjectItem> lst = new List<DispatchToObjectItem>();
            int totalCount;
            lst = dispatchToSV.GetAllObjectVerifyByID(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(lst, totalCount);
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
        public ActionResult CheckDeptByID(int dispatchID, int depID)
        {
            try
            {
                bool ck = dispatchToSV.CheckExitDepatmentInDispatchTo(dispatchID, depID);
                return this.Direct(ck);
            }
            catch (Exception)
            {
                return this.Direct(false);
            }
        }
        [ValidateInput(false)]
        public ActionResult Insert(DispatchToItem obj, [Bind(Prefix = "StorageFormID")]string[] borders)
        {
            setDocIssForm(borders, ref obj);
            string ck = chekValid(obj);
            if (ck.Equals(""))
            {
                int id = insert(obj);
                if (id > 0)
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("StDispatchToDepartment").Reload();
                }
                return this.Direct(true);
            }
            else
            {
                Ultility.ShowMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
                return this.Direct(false);
            }

        }
        [ValidateInput(false)]
        public ActionResult Update(DispatchToItem obj, [Bind(Prefix = "StorageFormID")]string[] borders)
        {

            setDocIssForm(borders, ref obj);
            string ck = chekValid(obj, obj.ID);

            if (ck.Equals(""))
            {
                if (update(obj))
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>("StDispatchToDepartment").Reload();
                }
                else
                    Ultility.CreateNotification(message: RequestMessage.UpdateError);

            }
            else
                Ultility.ShowMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
            return this.Direct();
        }
        public ActionResult MoveDispatch(DispatchToItem obj, string data = "")
        {

            if (!data.Trim().Equals(""))
            {
                bool ck = update(obj, true, false, false, data);
                if (ck)
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);

                    if (obj.Type.Equals(DispatchObjectType.Department.ToString()))
                    {
                        X.GetCmp<Store>("stDept").Reload();
                    }
                    else
                        X.GetCmp<Store>("stEmployee").Reload();
                    X.GetCmp<Store>("StDispatchToDepartment").Reload();
                }
                else
                    Ultility.CreateNotification(message: RequestMessage.UpdateError);
            }
            return this.Direct();
        }
        public ActionResult VerifyDispatch(DispatchToItem obj, bool isEmployee, string fName, string store = "")
        {

            bool ck = update(obj, false, true, isEmployee);
            if (ck)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                if (!store.Equals(""))
                    X.GetCmp<Store>(store).Reload();
                else
                    X.GetCmp<Store>("StDispatchToDepartment").Reload();
                X.GetCmp<Window>(fName).Close();
            }
            return this.Direct();
        }
        public ActionResult DeleteDepartment(int id)
        {
            dispatchToSV.DeleteDepatment(id);
            Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            return this.Direct();
        }
        public ActionResult DeleteEmployee(int id)
        {
            dispatchToSV.DeleteEmployee(id);
            Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
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
        private int insert(DispatchToItem obj)
        {
            int id = 0;

            obj.CreateBy = User.ID;
            id = dispatchToSV.Insert(obj);


            return id;
        }
        private bool update(DispatchToItem obj, bool isMove = false, bool isVerify = false, bool isEmployee = false, string data = "")
        {

            obj.UpdateBy = User.ID;
            if (isMove)
            {
                data = data.Trim(',');
                var attachments = JSON.Deserialize<List<int>>(data);
                var lstEmNotify = dispatchToSV.GetEmployeeNotify(obj.ID, attachments);
                dispatchToSV.UpdateMove(obj, attachments, User.ID);
                if (obj.Type.Equals(iDAS.Items.DispatchObjectType.Employee.ToString()))
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một công văn đã được chuyển đến bạn yêu cầu xác nhận!", obj.Name, lstEmNotify, User, Common.DispatchToEmployee, "DispatchID:" + obj.ID.ToString());
                }
            }
            else if (isVerify)
            {
                dispatchToSV.UpdateVerify(obj, isEmployee);
                if (obj.SendBy != 0)
                {
                    NotifyController notify = new NotifyController();
                    notify.NotifyUser(this.ModuleCode, "Có một công văn đã xác nhận!", obj.Name, obj.SendBy, User, Common.DispatchToDepartment, "DispatchID:" + obj.ID.ToString());
                }
            }
            else
                dispatchToSV.Update(obj);
            return true;
        }
        private string chekValid(DispatchToItem obj, int id = 0)
        {
            obj.NumberTo = obj.NumberTo.Trim();
            var ck = dispatchToSV.CheckNotExitNumberTo(obj.NumberTo, id);
            if (!ck)
                return "Số đến của công văn đã tồn tại. Vui lòng nhập Số đến khác!";

            return "";
        }
        private int setDocIssForm(string[] doc, ref DispatchToItem obj)
        {
            //string[] ar = ConvertData.SplitString(doc, ',');
            if (doc != null)
            {
                if (doc.Count() == 2)
                {
                    obj.FormH = true; obj.FormS = true;
                    return (int)StorageForm.Both;
                }
                else
                {
                    if (doc[0].ToString().Equals(StorageForm.SoftCopy.ToString()))
                    {
                        obj.FormS = true;
                        return (int)StorageForm.SoftCopy;
                    }
                    else
                    {
                        obj.FormH = true;
                        return (int)StorageForm.HardCopy;
                    }

                }

            }
            return -1;

        }
        public ActionResult ShowEmployeeCC(int dispatchToId = 0)
        {
            var obj = dispatchToSV.GetByID(dispatchToId);
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
        public ActionResult LoadEmployeeCC(StoreRequestParameters parameters, int dispatchToId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lsEmployee = dispatchToSV.GetEmployeeCC(filter, dispatchToId);
            return this.Store(lsEmployee, filter.PageTotal);
        }
        public ActionResult AddCC(int dispatchToId = 0, int employeeId = 0)
        {
            dispatchToSV.AddEmployeeCC(dispatchToId, employeeId, User.ID);
            var obj = dispatchToSV.GetByID(dispatchToId);
            NotifyController notify = new NotifyController();
            notify.Notify(this.ModuleCode, "Có một công văn đến bạn có quyền theo dõi", obj.Name, employeeId, User, Common.DispatchToCC, "DispatchID:" + dispatchToId);
            X.GetCmp<Store>("StoreEmployeeCC").Reload();
            return this.Direct();
        }
        public ActionResult DeleteCC(string stringId)
        {
            try
            {
                dispatchToSV.DeleteCC(stringId);
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
