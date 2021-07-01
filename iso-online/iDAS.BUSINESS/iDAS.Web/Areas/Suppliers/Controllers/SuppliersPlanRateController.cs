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
using iDAS.Utilities;
using iDAS.Web.Controllers;


namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [BaseSystem(Name = "Kế hoạch đánh giá NCC", Icon = "RubyAdd", IsActive = true, IsShow = true, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class SuppliersPlanRateController : BaseController
    {
        //
        // GET: /Suppliers/SuppliersPlanRate/

        private QualityPlanSV planSV = new QualityPlanSV();
        private SupplierSV suppSV = new SupplierSV();
        private SupplierAuditPlanSV suppAuditPlanSV = new SupplierAuditPlanSV();
        private SupplierAuditSV suppAuditSV = new SupplierAuditSV();

        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)       
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = suppAuditPlanSV.GetAll(filter,  focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm()
        {
            var data = new SupplierAuditPlanItem();
            data.ActionForm = Form.Add;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = new SupplierAuditPlanItem();
            if (id == 0)
            {
                data.ActionForm = Form.Add;
            }
            else
            {
                data = suppAuditPlanSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data, ViewData = new ViewDataDictionary { { "parentId", id } } };
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeletePlan(int id)
        {
            try
            {
                suppAuditPlanSV.Delete(id);
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
        public ActionResult DeleteSuppliersAudit(int id)
        {
            try
            {
                suppAuditSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreSuppAudit").Reload();
            }
            return this.Direct();
        }
        public ActionResult Update(SupplierAuditPlanItem data)
        {
            if (data.Approval.ID == 0)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn người phê duyệt kế hoạch này!"
                });
                return this.Direct(false);
            }
            else
            {

                try
                {
                    if (data.ID == 0)
                    {
                        data.CreateAt = DateTime.Now;
                        data.CreateBy = User.ID;
                        int id = suppAuditPlanSV.Insert(data);
                       int planId = suppAuditPlanSV.InsertSupplierPlan(id, User.ID);
                        if (!data.IsApproval && !data.IsEdit)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Có một kế hoạch đánh giá NCC mới cần phê duyệt", data.Name, data.Approval.ID, User, Common.SuppliersPlanRate, "SuppliersPlanRateID:" + planId.ToString());
                        }
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
                            suppAuditPlanSV.Update(data, User.ID);
                            suppAuditPlanSV.Update(data.ID, User.ID);
                            if (!data.IsApproval && !data.IsEdit)
                            {
                                NotifyController notify = new NotifyController();
                                notify.Notify(this.ModuleCode, "Có một kế hoạch đánh giá NCC mới cần phê duyệt", data.Name, data.Approval.ID, User, Common.SuppliersPlanRate, "SuppliersPlanRateID:" + data.ID.ToString());
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
                return this.Direct(true);
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new SupplierAuditPlanItem();
            data = suppAuditPlanSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        public ActionResult ApproveForm(int id = 0, int requirmentId = 0)
        {

            var data = new SupplierAuditPlanItem();
            if (id != 0)
            {
                data = suppAuditPlanSV.GetById(id);
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
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data, ViewData = new ViewDataDictionary { { "requirmentId", requirmentId } } };
            }
        }
        public ActionResult Appproved(SupplierAuditPlanItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    suppAuditPlanSV.Approve(updateData);
                    List<int> lstEmployeesID = new List<int>();
                    if (updateData.UpdateBy.HasValue && updateData.UpdateBy != updateData.ApprovalBy)
                    {
                        lstEmployeesID.Add(updateData.UpdateBy.Value);
                    }
                    if (updateData.CreateBy.HasValue && updateData.CreateBy != updateData.ApprovalBy)
                    {
                        lstEmployeesID.Add(updateData.CreateBy.Value);
                    }
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một kế hoạch phát triển sản phẩm đã được phê duyệt", updateData.Name, lstEmployeesID, User, Common.SuppliersPlanRate, "SuppliersPlanRateID:" + updateData.ID.ToString());
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
        public ActionResult ShowFrmFindSupplier()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmFindSuppliers" };
        }
        public ActionResult ShowSuppAuditList(int id=0)
        {
            SupplierAuditPlanItem data = suppAuditPlanSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SupplierAuditsLst",Model =data };
        }
        public ActionResult ShowSummaryResult(int id = 0)
        {
            SupplierAuditPlanItem data = new SupplierAuditPlanItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SummaryResult", Model = data };
        }
        public ActionResult GetListSupp(StoreRequestParameters parameters, int groupSupplierID = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<SupplierItem> lstData = new List<SupplierItem>();
            if (groupSupplierID != 0)
            {
                lstData = suppSV.GetAllByGroupID(filter, groupSupplierID);
            }
            return this.Store(lstData, filter.PageTotal);

        }
        public ActionResult GetSuppAudit(StoreRequestParameters parameters,int planID)
        {
            try
            {
                int total;
                var data = suppAuditSV.GetAllByPlanId(parameters.Page, parameters.Limit,out total, planID);
                return this.Store(data,total);
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }

        }
        public ActionResult GetSuppAuditSummaryResult(StoreRequestParameters parameters, int planID)
        {
            try
            {
                int total;
                var data = suppAuditSV.GetSummaryByPalnID(parameters.Page, parameters.Limit, out total, planID);
                return this.Store(data, total);
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }

        }
        public ActionResult updateSupplierAudit(int planid,string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');
                var data = suppAuditSV.GetAllByPlanId(planid);
                foreach(string suppId in arrayId)
                {
                    int _supID = int.Parse(suppId);
                    if (!data.Contains(_supID))
                        suppAuditSV.Insert(new SupplierAuditItem {SupplierAuditPlanID = planid,SupplierID=_supID });
                }
                
                
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
            finally
            {
                X.GetCmp<Store>("StoreSuppAudit").Reload();
                X.GetCmp<Window>("frmProducts").Close();
            }
            return this.Direct();
        }     

    }
}
