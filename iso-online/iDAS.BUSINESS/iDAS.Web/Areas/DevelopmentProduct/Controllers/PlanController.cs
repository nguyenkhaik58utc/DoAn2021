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

namespace iDAS.Web.Areas.DevelopmentProduct.Controllers
{
    [BaseSystem(Name = "Kế hoạch phát triển sản phẩm", Icon = "RubyAdd", IsActive = true, IsShow = true, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class PlanController : BaseController
    {
        //
        // GET: /DevelopmentProduct/Plan/
        private QualityPlanSV planSV = new QualityPlanSV();
        private ProductDevelopmentRequirementSV productDevelopmentRequirementSV = new ProductDevelopmentRequirementSV();
        private ProductDevelopmentRequirementPlanSV productDevelopmentRequirementPlanSV = new ProductDevelopmentRequirementPlanSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int requirmentId = 0, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = productDevelopmentRequirementPlanSV.GetAll(filter, requirmentId, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataRequirment(StoreRequestParameters parameters)
        {
            List<ProductDevelopmentRequirementItem> lstData;
            int total;
            lstData = productDevelopmentRequirementSV.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<ProductDevelopmentRequirementItem>(lstData, total));
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm(int requirmentId = 0)
        {
            var data = new ProductDevelopmentRequirementPlanItem();
            data.ActionForm = Form.Add;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data, ViewData = new ViewDataDictionary { { "requirmentId", requirmentId } } };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0, int requirmentId = 0)
        {
            var data = new ProductDevelopmentRequirementPlanItem();
            if (id == 0)
            {
                data.ActionForm = Form.Add;
            }
            else
            {
                data = productDevelopmentRequirementPlanSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data, ViewData = new ViewDataDictionary { { "requirmentId", requirmentId }, { "parentId", id } } };
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeletePlan(int id)
        {
            try
            {
                productDevelopmentRequirementPlanSV.DeleteByPlanID(id);
                planSV.Delete(id);
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
        public ActionResult Update(ProductDevelopmentRequirementPlanItem data)
        {
            if (data.Approval.ID == 0)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn người phê duyệt kế hoạch phát triển sản phẩm!"
                });
                return this.Direct(false);
            }
            else
            {

                try
                {
                    if (data.ID == 0)
                    {
                        if (planSV.CheckExits(data.Name))
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Thông báo",
                                Message = "Tên kế hoạch đã tồn tại, Vui lòng nhập tên khác!"
                            });
                        }
                        else
                        {
                            data.CreateAt = DateTime.Now;
                            data.CreateBy = User.ID;
                            int id = productDevelopmentRequirementSV.Insert(data);
                            productDevelopmentRequirementPlanSV.Insert(id, data.ProductDevelopmentRequirementID, User.ID);
                            if (!data.IsApproval && !data.IsEdit)
                            {
                                NotifyController notify = new NotifyController();
                                notify.Notify(this.ModuleCode, "Có một kế hoạch phát triển sản phẩm mới cần phê duyệt", data.Name, data.Approval.ID, User, Common.PlanProductionDevlopment, "PlanID:" + id.ToString());
                            }
                            Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        }

                    }
                    else
                    {
                        if (planSV.CheckEditExits(data.ID, data.Name))
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Thông báo",
                                Message = "Tên kế hoạch đã tồn tại, Vui lòng nhập tên khác!"
                            });
                        }
                        else
                        {
                            if (data.IsSendApproval && (data.Approval.ID == 0 || data.Approval == null))
                            {
                                Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt kế hoạch", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                            }
                            else
                            {
                                productDevelopmentRequirementSV.Update(data, User.ID);
                                productDevelopmentRequirementPlanSV.Update(data.ID, data.ProductDevelopmentRequirementID, User.ID);
                                if (!data.IsApproval && !data.IsEdit)
                                {
                                    NotifyController notify = new NotifyController();
                                    notify.Notify(this.ModuleCode, "Có một kế hoạch phát triển sản phẩm cần phê duyệt", data.Name, data.Approval.ID, User, Common.PlanProductionDevlopment, "PlanID:" + data.ID.ToString());
                                }
                                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                            }
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
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new ProductDevelopmentRequirementPlanItem();
            data = productDevelopmentRequirementPlanSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        public ActionResult ApproveForm(int id = 0, int requirmentId = 0)
        {

            var data = new ProductDevelopmentRequirementPlanItem();
            if (id != 0)
            {
                data = productDevelopmentRequirementPlanSV.GetById(id);
            }
            if (requirmentId == 0)
            {
                requirmentId = data.ProductDevelopmentRequirementID;
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
        public ActionResult Appproved(ProductDevelopmentRequirementPlanItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    productDevelopmentRequirementPlanSV.ApproveFromDevelopmentProduct(updateData);
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
                    notify.Notify(this.ModuleCode, "Có một kế hoạch phát triển sản phẩm đã được phê duyệt", updateData.Name, lstEmployeesID, User, Common.PlanProductionDevlopment, "PlanID:" + updateData.ID.ToString());
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

    }
}
