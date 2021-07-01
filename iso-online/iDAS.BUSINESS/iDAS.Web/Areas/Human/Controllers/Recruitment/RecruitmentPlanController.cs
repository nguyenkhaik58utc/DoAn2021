using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;


namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kế hoạch", Icon = "PageEdit", IsActive = true, IsShow = true, Position = 3, Parent = GroupRecruitmentController.Code)]
    public class RecruitmentPlanController : BaseController
    {
        private HumanRecruitmentPlanSV PlanService = new HumanRecruitmentPlanSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private TaskSV taskSV = new TaskSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult LoadPlanApproved(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = PlanService.GetAllApprovedSuccess(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }
        public ActionResult LoadPlan(StoreRequestParameters parameters, int focusId = 0, int departmentID = 0)
        {
            int totalCount;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            if (new DepartmentSV().GetByID(departmentID).ParentID == 0)
            {
                return this.Store(PlanService.GetAll(filter, focusId), filter.PageTotal);
            }
            else
            {
                return this.Store(PlanService.GetByDepartmentID(parameters.Page, parameters.Limit, out totalCount, departmentID), totalCount);
            }
        }
        // Lấy danh sách những yêu cầu tuyển dụng đã được phê duyệt
        public ActionResult LoadRequirementApproved(StoreRequestParameters parameters)
        {
            int totalCount;
            var Data = PlanService.RequirementApproved(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(Data, totalCount);
        }
        #region Thêm
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Add" };
        }
        [HttpPost]
        public ActionResult Insert(HumanRecruitmentPlanItem AddData, string listRequirementId)
        {
            try
            {
                var Id = 0;
                AddData.strRequirmentIDs = listRequirementId;
                // Lưu thông tin kế hoạch tuyển dụng
                PlanService.Insert(AddData, User.ID, out Id);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {

                X.GetCmp<Window>("winPlanAdd").Close();
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Sửa
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int ID = 0)
        {
            var data = new HumanRecruitmentPlanItem();
            if (ID != 0)
            {
                data = PlanService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };

        }
        [HttpPost]
        public ActionResult Update(HumanRecruitmentPlanItem updateData)
        {
            try
            {
                if (updateData.ID != 0)
                {
                    PlanService.UpdateInfo(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winPlanUpdate").Close();
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.Direct();
        }
        //Lấy danh sách yêu cầu của kế hoạch
        public ActionResult LoadRequirementUpdate(StoreRequestParameters parameters, int Id)
        {
            int totalCount;
            var Data = PlanService.GetRequirementSelect(parameters.Page, parameters.Limit, out totalCount, Id);
            return this.Store(Data, totalCount);
        }
        // Cập nhật yêu cầu tuyển dụng
        public ActionResult UpdateRequirement(string data)
        {
            var PlanRequirementdata = Ext.Net.JSON.Deserialize<HumanRecruitmentPlanRequirementItem>(data);
            try
            {
                PlanService.UpdateRequired(PlanRequirementdata);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirement").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        /// <summary>
        /// Xóa kế hoạch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeletePlan(int id)
        {
            try
            {
                PlanService.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Chi tiết
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID)
        {
            var data = PlanService.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadRequirementDetail(StoreRequestParameters parameters, int Id)
        {
            int totalCount;
            var Data = PlanService.GetRequirement(parameters.Page, parameters.Limit, out totalCount, Id);
            return this.Store(Data, totalCount);
        }
        #endregion

        #region Gửi duyệt
        [BaseSystem(Name = "Gửi duyệt")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult SendApprove(int ID = 0)
        {
            var data = new HumanRecruitmentPlanItem();
            if (ID != 0)
            {
                data = PlanService.GetById(ID);
            }
            if (data.IsEdit)
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "SendApprove", Model = data };
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult SendApprove(HumanRecruitmentPlanItem updateData, bool IsEdit = false)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    PlanService.SendApproval(updateData);
                    Ultility.CreateNotification(message: updateData.IsEdit ? RequestMessage.UpdateSuccess : RequestMessage.SendSuccess);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một kế hoạch tuyển dụng nhân sự cần phê duyệt", updateData.CreateByName, updateData.Approval.ID, User, Common.RecruitmentPlan, "RecruitmentPlanID:" + updateData.ID.ToString());
                    
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Phê duyệt
        [BaseSystem(Name = "Phê duyệt")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Approve(int ID = 0)
        {
            var data = new HumanRecruitmentPlanItem();
            if (ID != 0)
            {
                data = PlanService.GetById(ID);
            }
            if (!data.IsEdit)
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Approve(HumanRecruitmentPlanItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    updateData.IsAccept = updateData.IsResult == true;
                    PlanService.Approve(updateData);
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Kế hoạch tuyển dụng nhân sự của bạn đã được phê duyệt", updateData.CreateByName, updateData.CreateUserID, User, Common.RecruitmentPlan, "TrainingRequirementID:" + updateData.ID.ToString());
                    
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();

            }
            return this.FormPanel(success);
        }
        #endregion

        #region Giao việc
        [BaseSystem(Name = "Giao việc")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult TaskForm(int ID)
        {
            ViewData["PlanID"] = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Task", ViewData = ViewData };
        }
        /// <summary>
        /// Thêm mới công việc kế hoạch
        /// </summary>
        /// <param name="objNew"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public ActionResult InsertTask(HumanRecruitmentPlanTaskItem objNew, int planId, string arrObject = null)
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
                    objNew.CreateBy = User.ID;
                    int taskId = taskSV.Insert(objNew, User.ID, User.EmployeeID);
                    objNew.TaskID = taskId;
                    objNew.PlanID = planId;
                    PlanService.InsertTask(objNew, User.ID);
                    if (!objNew.IsEdit && !objNew.IsNew)
                    {
                        NotifyController notify = new NotifyController();
                        notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", objNew.Name, objNew.Perform.ID, User, Common.Task, "RecuitmentPlanTaskID:" + taskId.ToString());
                    }
                    if (!string.IsNullOrEmpty(arrObject))
                    {
                        var ids = arrObject.Split(',').Select(n => (object)int.Parse(n)).ToList();
                        ids.Remove(objNew.Perform.ID);
                        foreach (var ide in ids)
                        {
                            objNew.Perform = employeeSV.GetEmployeeView((int)ide);
                            objNew.Name = nameOld + " (" + objNew.Perform.Name + ")";
                            int taskIds = taskSV.Insert(objNew, User.ID, User.EmployeeID);
                            if (!objNew.IsEdit && !objNew.IsNew)
                            {
                                NotifyController notify = new NotifyController();
                                notify.Notify(this.ModuleCode, "Yêu cầu thực hiện công việc", objNew.Name, objNew.Perform.ID, User, Common.Task, "RecuitmentPlanTaskID:" + taskIds.ToString());
                            }
                            objNew.TaskID = taskIds;
                            objNew.PlanID = planId;
                            PlanService.InsertTask(objNew, User.ID);
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
        //public ActionResult LoadPlanTask(StoreRequestParameters parameters, int planId)
        //{
        //    int totalCount;
        //    var result = PlanService.GetTaskByPlanID(parameters.Page, parameters.Limit, out totalCount, planId);
        //    return this.Store(result, totalCount);
        //}
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
        public ActionResult LoadPlanTask(string node, int planId = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = PlanService.GetTreeTask(taskId, planId);
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
        #endregion
    }
}
