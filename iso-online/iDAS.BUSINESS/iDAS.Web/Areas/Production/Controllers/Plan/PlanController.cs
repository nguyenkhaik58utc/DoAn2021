using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Production.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kế hoạch", IsActive = true, IsShow = true, Position = 3, Icon = "TableEdit")]
    public class PlanController : BaseController
    {
        private ProductionPlanSV ProductionPlanSV = new ProductionPlanSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int requirementId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionPlanSV.GetByRequirement(filter, requirementId);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, string productionName = "", string requirementLevel = "", int requirementId = 0,
            string requirementColor = "", int quantity = 0, DateTime? endTime = null, int productionId = 0)
        {
            var data = new ProductionPlanItem();
            if (id != 0)
            {
                data = ProductionPlanSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ProductionRequirement = new ProductionRequirementViewItem()
                {
                    ID = requirementId,
                    Color = requirementColor,
                    Level = requirementLevel,
                    Quantity = quantity,
                    EndTime = (DateTime)endTime
                };
                data.Product = new ProductViewItem() { ID = productionId, Name = productionName };
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(ProductionPlanItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    ProductionPlanSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    data.ID = ProductionPlanSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: data.ID != 0 ? RequestMessage.UpdateError : RequestMessage.CreateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlan").Reload();
            }
            return this.Direct(data.ID);
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    ProductionPlanSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StorePlan").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new ProductionPlanItem();
            if (id != 0)
            {
                data = ProductionPlanSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Bán thành phẩm
        public ActionResult LoadProductionResult(StoreRequestParameters parameters, int planId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionPlanSV.GetProducResult(filter, planId);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult CalculatorStage(int planId, int productionId, int quantity)
        {
            try
            {
                ProductionPlanSV.CalculatorStage(planId, productionId, quantity);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("SemiProductStoreID").Reload();
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.Error, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        public ActionResult PlanStage(int planProductId, int stageId, int quantity, DateTime startDate, DateTime endDate,bool isReadOnly = false)
        {
            ViewData["PlanProductID"] = planProductId;
            ViewData["StageID"] = stageId;
            ViewData["Quantity"] = quantity;
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;
            ViewData["ReadOnly"] = isReadOnly;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "PlanStage", ViewData = ViewData };
        }
        public ActionResult LoadPlanStage(int planProductId = 0)
        {
            var result = ProductionPlanSV.GetPlanStage(planProductId);
            return this.Store(result);
        }
        public ActionResult CaculatorPlan(int planProductId, int stageId, int quantity, DateTime startDate, DateTime endDate, int departmentId)
        {
            try
            {
                ProductionPlanSV.CaculatorPlan(planProductId, stageId, quantity, startDate, endDate, departmentId,User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("PlanStageStoreID").Reload();
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.Error, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        [HttpGet]
        public ActionResult SemiProduct(int id)
        {
            ProductionPlanProductItem data = ProductionPlanSV.GetSemiProductById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "SemiProduct", ViewData = ViewData, Model = data };
        }
        [HttpPost]
        public ActionResult SemiProduct(ProductionPlanProductItem data)
        {
            try
            {
                ProductionPlanSV.UpdateSemiProduct(data, User.ID);
                X.GetCmp<Store>("SemiProductStoreID").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        public ActionResult SemiProductDelete(int id)
        {
            try
            {
                ProductionPlanSV.DeleteSemiProduct(id);
                X.GetCmp<Store>("SemiProductStoreID").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Thiết bị
        public ActionResult LoadEquipment(int planId)
        {
            var result = ProductionPlanSV.GetEquipment(planId);
            return this.Store(result);
        }
        [HttpGet]
        public ActionResult Equipment(int id = 0, int planId = 0)
        {
            var data = new ProductionPlanEquipmentItem();
            if (id != 0)
            {
                data = ProductionPlanSV.GetEquipmentById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ProductionPlanID = planId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Equipment", Model = data };
        }
        [HttpPost]
        public ActionResult Equipment(ProductionPlanEquipmentItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    ProductionPlanSV.UpdateEquipment(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    ProductionPlanSV.InsertEquipment(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: data.ID != 0 ? RequestMessage.UpdateError : RequestMessage.CreateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("EquipmentStoreID").Reload();
            }
            return this.Direct();
        }
        public ActionResult EquipmentDelete(int id)
        {
            try
            {
                ProductionPlanSV.DeleteEquipment(id);
                X.GetCmp<Store>("EquipmentStoreID").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Nguyên vật liệu
        public ActionResult LoadMaterial(int planId)
        {
            var result = ProductionPlanSV.GetMaterial(planId);
            return this.Store(result);
        }
        [HttpGet]
        public ActionResult Material(int id = 0, int planId = 0)
        {
            var data = new ProductionPlanMaterialItem();
            if (id != 0)
            {
                data = ProductionPlanSV.GetMaterialById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ProductionPlanID = planId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Material", Model = data };
        }
        [HttpPost]
        public ActionResult Material(ProductionPlanMaterialItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    ProductionPlanSV.UpdateMaterial(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    ProductionPlanSV.InsertMaterial(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: data.ID != 0 ? RequestMessage.UpdateError : RequestMessage.CreateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("MaterialStoreID").Reload();
            }
            return this.Direct();
        }
        public ActionResult MaterialDelete(int id)
        {
            try
            {
                ProductionPlanSV.DeleteMaterial(id);
                X.GetCmp<Store>("MaterialStoreID").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Dùng chung
        public ActionResult LoadPlanForCommand(int requirementId = 0, int departmentId = 0)
        {
            var result = ProductionPlanSV.GetPlanForCommand(requirementId, departmentId);
            return this.Store(result);
        }
        #endregion
    }
}

