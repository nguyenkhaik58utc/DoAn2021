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
    [BaseSystem(Name = "Công đoạn sản xuất", IsActive = true, IsShow = true, Position = 4, Icon = "TagBlue", Parent = GroupCategoryController.Code)]
    public class StageController : BaseController
    {
        private ProductionStageSV ProductionStageSV = new ProductionStageSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionStageSV.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0)
        {
            var data = new ProductionStageItem();
            if (id != 0)
            {
                data = ProductionStageSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
                data.Equipements = new List<ProductionStageEquipmentItem>();
                data.Materials = new List<ProductionStageMaterialItem>();
                data.Products = new List<ProductionStageProductItem>();
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(ProductionStageItem data, string strEquipement, string strMaterial, string strProduct)
        {
            try
            {
                data.Equipements = Ext.Net.JSON.Deserialize<List<ProductionStageEquipmentItem>>(strEquipement);
                data.Materials = Ext.Net.JSON.Deserialize<List<ProductionStageMaterialItem>>(strMaterial);
                data.Products = Ext.Net.JSON.Deserialize<List<ProductionStageProductItem>>(strProduct);
                if (data.ID != 0)
                {
                    ProductionStageSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    ProductionStageSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreStage").Reload();
            }
            return this.Direct();
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
                    ProductionStageSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StoreStage").Reload();
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
            var data = new ProductionStageItem();
            if (id != 0)
            {
                data = ProductionStageSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Dùng chung
        public ActionResult EquipmentWindow(bool isUpdate = false)
        {
            ViewData["Update"] = isUpdate;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "EquipmentWindow", ViewData = ViewData };
        }
        public ActionResult EquipmentWindowSelect()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "EquipmentWindowSelect" };
        }
        public ActionResult MaterialWindow(bool isUpdate = false)
        {
            ViewData["Update"] = isUpdate;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "MaterialWindow", ViewData = ViewData };
        }
        public ActionResult MaterialWindowSelect()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "MaterialWindowSelect" };
        }
        public ActionResult LoadMaterial(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductionStageSV.GetMaterial(filter);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult ProductWindow(int StockProductID = 0, string ProductName = "", int Quantity = 0, bool isUpdate = false)
        {
            ViewData["Update"] = isUpdate;
            var data = new ProductionStageProductItem();
            if (isUpdate)
            {
                data.StockProductID = StockProductID;
                data.ProductName = ProductName;
                data.Quantity = Quantity;
            }
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ProductWindow", ViewData = ViewData, Model = data };
        }
        public ActionResult LoadByProduct(int productId)
        {
            var result = ProductionStageSV.GetByProduct(productId);
            return this.Store(result);
        }
        #endregion
    }
}
