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
    [BaseSystem(Name = "Kế hoạch cung cấp dịch vụ", Icon = "RubyAdd", IsActive = true, IsShow = true, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class PlanController : BaseController
    {
        //
        // GET: /Service/Plan/
        private ServicePlanSV planService = new ServicePlanSV();
        private ServiceCommandService commandService = new ServiceCommandService();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int commandId = 0, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = planService.GetAll(filter, commandId, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataByParent(StoreRequestParameters parameters, int serviceCommandContractID = 0, int parentId = 0)
        {
            List<ServicePlanItem> lstData = new List<ServicePlanItem>();
            int total = 0;
            if (parentId != 0)
            {
                lstData = planService.GetByParentID(parameters.Page, parameters.Limit, out total, serviceCommandContractID, parentId);
            }
            return this.Store(new Paging<ServicePlanItem>(lstData, total));
        }
        public ActionResult GetDataIsApproval(StoreRequestParameters parameters, Nullable<DateTime> fromdate = null, Nullable<DateTime> todate = null, string year = "")
        {
            List<ServicePlanItem> lstData;
            int total;
            lstData = planService.GetAllIsApproval(parameters.Page, parameters.Limit, out total, fromdate, todate, year);
            return this.Store(new Paging<ServicePlanItem>(lstData, total));
        }
        public ActionResult GetDataContractDetail(int serviceCommandContractID = 0)
        {
            List<ServiceCommandContractItem> lstData;
            lstData = planService.GetCommandContractByID(serviceCommandContractID);
            return this.Store(lstData);
        }
        public ActionResult GetDataContract(int commandId = 0)
        {
            List<ServiceCommandContractItem> lstData;
            lstData = planService.GetByCommandID(commandId);
            return this.Store(lstData);
        }
        public ActionResult GetDataServiceStage(int contractId = 0, int customerId = 0, int servicePlanId = 0)
        {
            List<ServiceStageItem> lstData;
            lstData = planService.GetStageByContractID(contractId, customerId, servicePlanId);
            return this.Store(lstData);
        }
        public ActionResult GetDataServiceStageForDetail(int contractId = 0, int customerId = 0, int servicePlanId = 0)
        {
            List<ServiceStageItem> lstData;
            lstData = planService.GetStageByContractIDForDetail(contractId, customerId, servicePlanId);
            return this.Store(lstData);
        }
        public ActionResult AddNewPlanDetail(int serviceCommandContractID = 0, int parentId = 0)
        {
            ServicePlanItem obj = new ServicePlanItem();
            obj.ServiceCommandContractID = serviceCommandContractID;
            obj.ParentID = parentId;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertPlanDetail", Model = obj, ViewData = new ViewDataDictionary { { "parentId", parentId } } };
        }
        public ActionResult FrmUpdatePlanDetail(int id = 0, int commandId = 0)
        {
            var data = planService.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdatePlanDetail", Model = data, ViewData = new ViewDataDictionary { { "commandId", commandId } } };
        }
        public ActionResult FrmViewPlanDetail(int id = 0, int commandId = 0)
        {
            var data = planService.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ViewPlanDetail", Model = data, ViewData = new ViewDataDictionary { { "commandId", commandId } } };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm(int commandId = 0)
        {
            var data = new ServicePlanItem();
            data.EndTime = commandId != 0 ? commandService.GetByID(commandId).EndTime.Value : new DateTime();
            data.ActionForm = Form.Add;
            data.Approval = new HumanEmployeeSV().GetEmployeeView(commandService.GetByID(commandId).ApprovalBy);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data, ViewData = new ViewDataDictionary { { "commandId", commandId } } };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0, int commandId = 0)
        {
            var data = new ServicePlanItem();
            if (id == 0)
            {
                data.ActionForm = Form.Add;
            }
            else
            {
                data = planService.GetById(id);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data, ViewData = new ViewDataDictionary { { "commandId", commandId }, { "parentId", id } } };
        }
        public ActionResult ApproveForm(int id = 0, int commandId = 0)
        {
            var data = new ServicePlanItem();
            if (id != 0)
            {
                data = planService.GetById(id);
            }
            if (data.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt kế hoạch này!"
                });
                return this.Direct();
            }
            else
            {
             
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data, ViewData = new ViewDataDictionary { { "commandId", commandId }, { "parentId", id } } };
            }

        }
        public ActionResult Appproved(ServicePlanItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    planService.ApproveFromService(updateData);
                    List<int> lstEmployeesID = new List<int>();
                    if (updateData.UpdateByEmployeeID.HasValue && updateData.UpdateByEmployeeID != updateData.ApprovalBy)
                    {
                        lstEmployeesID.Add(updateData.UpdateByEmployeeID.Value);
                    }
                    if (updateData.CreateByEmployeeID.HasValue && updateData.CreateByEmployeeID != updateData.ApprovalBy)
                    {
                        lstEmployeesID.Add(updateData.CreateByEmployeeID.Value);
                    }
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một kế hoạch cung cấp dịch vụ đã được phê duyệt", updateData.Name, lstEmployeesID, User, Common.PlanService, "PlanID:" + updateData.ID.ToString());
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlanIndex").Reload();

            }
            return this.FormPanel(success);
        }
        public ActionResult UpdateByParent(ServicePlanItem data, string strStageID, int customerId)
        {
            if (!string.IsNullOrEmpty(strStageID))
            {
                try
                {
                    if (data.ID == 0)
                    {
                        data.CreateAt = DateTime.Now;
                        data.CreateBy = User.ID;
                        int id = planService.Insert(data);
                        planService.InsertPlanDetail(id, data.ServiceCommandContractID, User.ID, strStageID, customerId);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                    else
                    {
                        if (data.IsSendApproval && (data.Approval.ID == 0 || data.Approval == null))
                        {
                            Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt kế hoạch", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                        }
                        else
                        {
                            planService.Update(data, User.ID);
                            planService.UpdatePlanDetail(data.ID, data.ServiceCommandContractID, User.ID, strStageID, customerId);
                            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);

                        }
                    }

                }
                catch
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

                }
                finally
                {
                    X.GetCmp<Store>("StorePlanInsert").Reload();
                }
                return this.Direct();
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn công đoạn thực hiện!"
                });
                return this.Direct();
            }
        }
        public ActionResult Update(ServicePlanItem data)
        {
            if (data.Approval.ID == 0)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn người phê duyệt kế hoạch cung cấp dịch vụ!"
                });
                return this.Direct(null);
            }
            else
            {
                int planId = 0;
                try
                {
                    if (data.ID == 0)
                    {
                        data.CreateAt = DateTime.Now;
                        data.CreateBy = User.ID;
                        int id = planService.Insert(data);
                       int idlans = planService.Insert(id, data.ServiceCommandContractID, User.ID);
                        if (!data.IsApproval && !data.IsEdit)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Có một kế hoạch cung cấp dịch vụ cần phê duyệt", data.Name, data.Approval.ID, User, Common.PlanService, "PlanID:" + idlans.ToString());
                        }
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        planId = id;
                    }
                    else
                    {
                        if (data.IsSendApproval && (data.Approval.ID == 0 || data.Approval == null))
                        {
                            Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt kế hoạch", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                        }
                        else
                        {
                            planService.Update(data, User.ID);
                            planService.Update(data.ID, data.ServiceCommandContractID, User.ID);
                            if (!data.IsApproval && !data.IsEdit)
                            {
                                NotifyController notify = new NotifyController();
                                notify.Notify(this.ModuleCode, "Có một kế hoạch cung cấp dịch vụ cần phê duyệt", data.Name, data.Approval.ID, User, Common.PlanService, "PlanID:" + data.ID.ToString());
                            }
                            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);

                        }
                    }

                }
                catch
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

                }
                finally
                {
                    X.GetCmp<Store>("StorePlanIndex").Reload();
                }
                return this.Direct(planId);
            }
        }
        public ActionResult SendAppproval(ServicePlanItem data)
        {

            try
            {
                planService.SendApproval(data, User.ID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một kế hoạch cung cấp dịch vụ cần phê duyệt", data.Name, data.ApprovalBy.Value, User, Common.PlanService, "PlanID:" + data.ID.ToString());
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            finally
            {
                X.GetCmp<Store>("StorePlanIndex").Reload();
            }
            return this.Direct();

        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeletePlan(int id)
        {
            try
            {
                planService.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlanIndex").Reload();
            }
            return this.Direct();
        }
        public ActionResult DeleteDetail(int id)
        {
            try
            {
                planService.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlanInsert").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0, int commandId = 0)
        {
            var data = planService.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data, ViewData = new ViewDataDictionary { { "commandId", commandId }, { "parentId", id } } };
        }
        [BaseSystem(Name = "Gửi duyệt", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult SendApprovalForm(int id = 0, int commandId = 0)
        {
            var data = planService.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SendApproval", Model = data, ViewData = new ViewDataDictionary { { "commandId", commandId }, { "parentId", id } } };
        }
    }
}
