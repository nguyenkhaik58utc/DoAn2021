using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Document.Controllers
{
    [BaseSystem(Name = "Đề nghị viết mới/sửa đổi tài liệu", Icon = "PageEdit", IsActive = true, IsShow = true, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RequestController : BaseController
    {
        private DocumentRequirementSV reqSV = new DocumentRequirementSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private DocumentSV docSV = new DocumentSV();
        private TaskSV taskSV = new TaskSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int groupId, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var requests = reqSV.GetAll(filter, groupId, focusId);
            return this.Store(requests, filter.PageTotal);
        }
        public ActionResult GetDocInfoByID(int id = 0)
        {
            if (id > 0)
            {
                var obj = docSV.GetByID(id);
                X.GetCmp<Hidden>("txtVersion").Text = obj.Version;
                X.GetCmp<Hidden>("txtTimes").Text = obj.IssuedTime.ToString();
            }
            return this.Direct();
        }
        /// <summary>
        /// Lấy danh mục loại yêu cầu
        /// </summary>
        /// <returns></returns>
        public ActionResult FillRequestType()
        {
            var lst = new List<DocumentSecurityItem>{
                new DocumentSecurityItem { ID = (int)RequestType.New, Name = "Viết mới" },
                new DocumentSecurityItem { ID=(int)RequestType.Modified, Name= "Sửa đổi"}
                        };
            return this.Store(lst);
        }
        public ActionResult FillDocumentIssued()
        {
            var lst = docSV.GetAllIssued();
            return this.Store(lst);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm")]
        [HttpGet]
        public ActionResult Insert()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
        }
        [HttpPost]
        public ActionResult Insert(DocumentRequirementItem obj, bool IsSendApproval = false, bool isreload = true)
        {
            obj.IsEdit = !IsSendApproval;
            obj.CreateBy = User.ID;
            int id =  reqSV.InsertRequest(obj);
            if (IsSendApproval)
            {
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một yêu cầu về tài liệu cần phê duyệt", obj.Name, obj.Approval.ID, User, Common.DocumentRequest, "DocumentRequestID:" + id.ToString());
            }
            Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            if (isreload)
            {
                X.GetCmp<Store>("StRequest").Reload();
                X.GetCmp<Window>("winNewRequest").Close();
            }
            return this.Direct();
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var obj = reqSV.GetByID(id);
            if (obj.IsObsolete)
            {
                Ultility.ShowMessageBox("Thông báo", "Tài liệu có yêu cầu sửa đổi đang ở trạng thái Lỗi thời, nên không được phép sửa yêu cầu này.", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                return this.Direct();
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        public ActionResult Update(DocumentRequirementItem obj, bool IsSendApproval = false)
        {
            obj.IsEdit = !IsSendApproval;
            obj.UpdateBy = User.ID;
            reqSV.Update(obj);
            if (IsSendApproval)
            {
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một yêu cầu về tài liệu cần phê duyệt", obj.Name, obj.Approval.ID, User, Common.DocumentRequest, "RequestID:" + obj.ID.ToString());
            }
            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            X.GetCmp<Store>("StRequest").Reload();
            X.GetCmp<Window>("winEditRequest").Close();
            return this.Direct();
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            reqSV.Delete(id);
            Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            X.GetCmp<Store>("StRequest").Reload();
            return this.Direct();
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var obj = reqSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Phê duyệt")]
        [HttpGet]
        public ActionResult Approve(int id)
        {
            var obj = reqSV.GetByID(id);
            if (obj.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt yêu cầu này!"
                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Approval", Model = obj };
            }
        }
        [HttpPost]
        public ActionResult Approve(DocumentRequirementItem obj)
        {
            obj.UpdateBy = User.ID;
            reqSV.UpdateApprove(obj);
            NotifyController notify = new NotifyController();
            notify.Notify(this.ModuleCode, "Có một yêu cầu về tài liệu đã phê duyệt", obj.Name, obj.EmployeesCreateID, User, Common.DocumentRequest, "RequestID:" + obj.ID.ToString());
            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            X.GetCmp<Store>("StRequest").Reload();
            X.GetCmp<Window>("winApproveRequest").Close();
            return this.Direct();
        }
        public ActionResult InsertModified(int id)
        {
            var obj = new DocumentRequirementItem { DocumentID = id, TypeID = (int)RequestType.Modified, Times = docSV.GetByID(id).IssuedTime.ToString(), Version = docSV.GetByID(id).Version.ToString() };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertModified ", Model = obj };
        }
        public ActionResult InsertTask(TaskItem objNew, int requestID = 0, string arrObject = null)
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
                    string nameOld = objNew.Name;
                    if (!string.IsNullOrEmpty(arrObject))
                    {
                        objNew.Perform = employeeSV.GetEmployeeView(objNew.Perform.ID);
                        objNew.Name = nameOld + " (" + objNew.Perform.Name + ")";
                    }
                    objNew.CreateBy = User.ID;
                    int taskId = taskSV.Insert(objNew, User.ID, User.EmployeeID);
                    reqSV.InsertTask(taskId, requestID, User.ID, objNew.Content);
                    if (!objNew.IsEdit && !objNew.IsNew)
                    {
                        NotifyController notify = new NotifyController();
                        notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", objNew.Name, objNew.Perform.ID, User, Common.Task, "RequestTaskID:" + taskId.ToString());
                    }
                    if (!string.IsNullOrEmpty(arrObject))
                    {
                        var ids = arrObject.Split(',').Select(n => (object)int.Parse(n)).ToList();
                        ids.Remove(objNew.Perform.ID);
                        foreach (var ide in ids)
                        {
                            objNew.CreateBy = User.ID;
                            objNew.Perform = employeeSV.GetEmployeeView((int)ide);
                            objNew.Name = nameOld + " (" + objNew.Perform.Name + ")";
                            int taskIds = taskSV.Insert(objNew, User.ID, User.EmployeeID);
                            if (!objNew.IsEdit && !objNew.IsNew)
                            {
                                NotifyController notify = new NotifyController();
                                notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", objNew.Name, objNew.Perform.ID, User, Common.Task, "RequestTaskID:" + taskIds.ToString());
                            }
                            reqSV.InsertTask(taskIds, requestID, User.ID, objNew.Content);
                        }
                    }
                    Ultility.ShowNotification(message: RequestMessage.CreateSuccess);
                    success = true;
                }
            }
            catch
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.FormPanel(success);
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
        public ActionResult LoadTasks(string node, int requestID = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = reqSV.GetTreeTask(taskId, requestID);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, requestID));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        //public ActionResult LoadTasks(StoreRequestParameters parameters, int requestID = 0)
        //{
        //    ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
        //    var lst = reqSV.GetAllTaskByID(filter, requestID);
        //    return this.Store(lst, filter.PageTotal);

        //}
        public Ext.Net.MVC.PartialViewResult Task(string containerId)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                ContainerId = containerId,
                ViewName = "Task",
                WrapByScriptTag = false,
                RenderMode = RenderMode.AddTo
            };
        }
    }
}
