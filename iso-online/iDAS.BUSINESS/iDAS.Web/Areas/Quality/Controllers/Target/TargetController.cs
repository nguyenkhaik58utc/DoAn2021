using Ext.Net;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Quản lý mục tiêu", IsActive = true, Icon = "ChartLine", IsShow = true, Position = 3, Parent = GroupMenuTargetController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TargetController : BaseController
    {
        private QualityTargetSV targetSV = new QualityTargetSV();
        private DepartmentSV departmentSV = new DepartmentSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            ViewBag.DepartmentId = focusId != 0 ? targetSV.GetByID(focusId).DepartmentID : 0;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int departmentId = 0, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var Task = targetSV.GetAll(filter, departmentId, focusId);
            return this.Store(Task, filter.PageTotal);
        }
        public ActionResult GetDataIsAccept(StoreRequestParameters parameters, int departmentId = 0)
        {
            int totalCount;
            var Task = targetSV.GetIsAccept(parameters.Page, parameters.Limit, out totalCount, departmentId);
            return this.Store(Task, totalCount);
        }
        public ActionResult GetTargetParent(StoreRequestParameters parameters)
        {
            var Task = targetSV.GetParentTarget();
            return this.Store(Task);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0, int targetId = 0, int departId = 0)
        {
            var data = new QualityTargetItem();
            ViewData["EmployeeId"] = User.EmployeeID;
            if (id == 0)
            {
                data.ActionForm = Form.Add;
                data.ParentID = targetId;
                if (departId != 0)
                {
                    data.Department = new HumanDepartmentViewItem
                    {
                        ID = departId,
                        Name = departmentSV.GetByID(departId).Name,

                    };
                }
                if (targetId != 0)
                {
                    data.MaxValueCompleteAt = targetSV.GetByID(targetId).CompleteAt;
                    data.CompleteAt = targetSV.GetByID(targetId).CompleteAt;
                    data.Approval = employeeSV.GetEmployeeView(targetSV.GetByID(targetId).ApprovalBy);
                }
            }
            else
            {
                data = targetSV.GetByID(id);
                if (data.ParentID.HasValue && data.ParentID.Value != 0)
                    data.MaxValueCompleteAt = targetSV.GetByID(data.ParentID.Value).CompleteAt;
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data, ViewData = ViewData };
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteTarget(int id)
        {
            try
            {
                targetSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stTarget").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = targetSV.GetByID(id) };
        }
        public ActionResult ShowFrmApproval(int id)
        {
            var obj = targetSV.GetByID(id);
            if (obj.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt mục tiêu này!"
                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Approval", Model = obj };
            }
        }
        public ActionResult ApproveTarget(QualityTargetItem obj)
        {
            bool success = false;
            try
            {
                if (obj.ID != 0)
                {
                    targetSV.Approve(obj, User.ID);
                    List<int> lstEmployeesID = new List<int>();
                    if (obj.UpdateByEmployeeID.HasValue && obj.UpdateByEmployeeID != obj.ApprovalBy)
                    {
                        lstEmployeesID.Add(obj.UpdateByEmployeeID.Value);
                    }
                    if (obj.CreateByEmployeeID.HasValue && obj.CreateByEmployeeID != obj.ApprovalBy)
                    {
                        lstEmployeesID.Add(obj.CreateByEmployeeID.Value);
                    }
                    NotifyController notify = new NotifyController();
                    obj.TargetName = "Đến ngày: " + obj.CompleteAt.ToString() + " " + iDAS.Utilities.Common.GetTypeName(obj.Type) + obj.Value.ToString() + " " + obj.Unit;
                    notify.Notify(this.ModuleCode, "Có một mục tiêu được phê duyệt", obj.TargetName, lstEmployeesID, User, Common.Target, "TargetID:" + obj.ID.ToString());
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            return this.FormPanel(success);
        }
        public ActionResult Update(QualityTargetItem objNew)
        {
            bool success = false;
            try
            {
                if (objNew.ID == 0)
                {
                    if ((objNew.Approval == null || objNew.Approval.ID == 0) && !objNew.IsEdit)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt mục tiêu!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                    else
                    {
                        int tagertId = targetSV.Insert(objNew, User.ID);
                        if (!objNew.IsEdit)
                        {
                            NotifyController notify = new NotifyController();
                            objNew.TargetName = "Đến ngày: " + objNew.CompleteAt.ToString() + " " + iDAS.Utilities.Common.GetTypeName(objNew.Type) + objNew.Value.ToString() + " " + objNew.Unit;
                            notify.Notify(this.ModuleCode, "Có một mục tiêu chờ phê duyệt", objNew.TargetName, objNew.Approval.ID, User, Common.Target, "TargetID:" + tagertId.ToString());
                        }
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                    success = true;
                }
                else
                {
                    if ((objNew.Approval == null || objNew.Approval.ID == 0) && !objNew.IsEdit)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt mục tiêu!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                    targetSV.Update(objNew, User.ID);
                    if (!objNew.IsEdit)
                    {
                        NotifyController notify = new NotifyController();
                        objNew.TargetName = "Đến ngày: " + objNew.CompleteAt.ToString() + " " + iDAS.Utilities.Common.GetTypeName(objNew.Type) + objNew.Value.ToString() + " " + objNew.Unit;
                        notify.Notify(this.ModuleCode, "Có một mục tiêu chờ phê duyệt", objNew.TargetName, objNew.Approval.ID, User, Common.Target, "TargetID:" + objNew.ID.ToString());
                    }
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.FormPanel(success);
        }
        public ActionResult TargetWindowView(string urlStore = "", int paramID = 0)
        {
            ViewData["UrlStore"] = urlStore;
            ViewData["ParamID"] = paramID;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        public ActionResult TargetListView(string urlStore = "", int paramID = 0)
        {
            ViewData["UrlStore"] = urlStore;
            ViewData["ParamID"] = paramID;
            return PartialView(ViewData = ViewData);
        }
        public ActionResult TargetAddView(string urlStore = "", int paramID = 0)
        {
            ViewData["UrlStore"] = urlStore;
            ViewData["ParamID"] = paramID;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "TargetAddView", ViewData = ViewData };
        }
        public ActionResult TargetUpdateView(int id, string urlStore = "", int paramID = 0)
        {
            ViewData["UrlStore"] = urlStore;
            ViewData["ParamID"] = paramID;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "TargetUpdateView", ViewData = ViewData, Model = targetSV.GetByID(id) };
        }
        /// <summary>
        /// Chi tiết danh sách mục tiêu
        /// </summary>
        /// <param name="urlStore"></param>
        /// <param name="paramID"></param>
        /// <returns></returns>
        public ActionResult TargetIndexView(string urlStore = "", int paramID = 0)
        {
            ViewData["UrlStore"] = urlStore;
            ViewData["ParamID"] = paramID;
            return PartialView(ViewData = ViewData);
        }

        #region Kết thúc mục tiêu
        [BaseSystem(Name = "Kết thúc", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult TargetEnd(int id)
        {
            var data = targetSV.GetTargetEnd(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "TargetEnd", Model = data };
        }
        [HttpPost]
        public ActionResult TargetEnd(QualityTargetItem target)
        {
            try
            {
                targetSV.End(target, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        #endregion

        #region Theo dõi mục tiêu
        public ActionResult Follow(int id)
        {
            var data = new QualityTargetItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Follow", Model = data };
        }
        public ActionResult LoadTarget(string node, int targetId = 0)
        {
            NodeCollection nodes = new NodeCollection();
            bool isRoot = node == "root" ? true : false;
            var parentId = isRoot ? targetId : System.Convert.ToInt32(node);
            var targets = targetSV.GetChildTarget(parentId, isRoot);
            foreach (var target in targets)
            {
                nodes.Add(creatTargets(target));
            }
            return this.Content(nodes.ToJson());
        }
        private Node creatTargets(QualityTargetItem dataNode)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            nodeItem.Text = dataNode.Name;
            nodeItem.Icon = Icon.TagBlue;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CompleteAt", Value = dataNode.CompleteAt.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        public ActionResult LoadPlan(string node, int targetId = 0)
        {
            NodeCollection nodes = new NodeCollection();
            bool isRoot = node == "root" ? true : false;
            var parentId = isRoot ? targetId : System.Convert.ToInt32(node);
            if (!isRoot || targetId != 0)
            {
                var targets = targetSV.GetChildPlan(parentId, isRoot);
                foreach (var target in targets)
                {
                    nodes.Add(creatPlan(target));
                }
            }
            return this.Content(nodes.ToJson());
        }
        private Node creatPlan(QualityPlanItem dataNode)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            nodeItem.Text = !dataNode.Leaf ? dataNode.Name.ToUpper() : dataNode.Name;
            nodeItem.Icon = Icon.TagBlue;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Status", Value = dataNode.Status.ToString(), Mode = ParameterMode.Value });
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        #endregion
        private Node createNodeTarget(QualityTargetItem dataNode, int departmentId, int focusId)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            if (dataNode.DepartmentID != departmentId)
            {
                nodeItem.Cls = "clsUnView";
            }
            nodeItem.Text = !dataNode.Leaf ? ("Đến ngày: " + dataNode.CompleteAt.ToShortDateString() + " " + dataNode.Name + " " + dataNode.Value.ToString() + " " + dataNode.Unit).ToUpper() : ("Đến ngày: " + dataNode.CompleteAt.ToShortDateString() + " " + dataNode.Name + " " + dataNode.Value.ToString() + " " + dataNode.Unit);
            nodeItem.Icon = !dataNode.Leaf ? Icon.Folder : Icon.ChartLine;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsEdit", Value = JSON.Serialize(dataNode.IsEdit), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CategoryName", Value = dataNode.CateName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsApproval", Value = JSON.Serialize(dataNode.IsApproval), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Status", Value = dataNode.Status.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelName", Value = dataNode.LevelName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelColor", Value = dataNode.LevelColor.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CompleteAt", Value = dataNode.CompleteAt.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            if (dataNode.ID == focusId)
            {
                nodeItem.Cls = "idas-focus";
            }
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        public ActionResult LoadTreeTargets(string node, int cateId = 0, int dpId = 0, Nullable<DateTime> fromdate = null, Nullable<DateTime> todate = null, string choise = "", int focusId = 0)
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
                var targetId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = targetSV.GetTreeTarget(targetId, cateId, dpId, User.GroupIDs, User.Administrator, User.EmployeeID, User.ID, fromdate, todate, choise, focusId);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTarget(task, dpId, focusId));
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
