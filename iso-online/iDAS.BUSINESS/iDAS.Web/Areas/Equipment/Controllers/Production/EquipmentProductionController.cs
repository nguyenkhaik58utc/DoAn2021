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
namespace iDAS.Web.Areas.Equipment.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thiết Bị Sản Xuất", IsActive = true, IsShow = true, Position = 2, Icon = "Table")]
    public class EquipmentProductionController : BaseController
    {
        public EquipmentProductionSV ProductionServices = new EquipmentProductionSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadProduction(StoreRequestParameters parameters, int categoryId = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentProductionItem>();
            if (categoryId != 0)
            {
                result = ProductionServices.GetByCategory(parameters.Page, parameters.Limit, out totalCount, categoryId);
            }
            return this.Store(result, totalCount);
        }
        public ActionResult GetCategory()
        {
            var result = new List<EquipmentProductionCategoryItem>();
            result = ProductionServices.GetCategory();
            return this.Store(result);
        }

        #region Cập nhật thiết bị sản xuất
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = new EquipmentProductionItem();
            if (id != 0)
            {
                data = ProductionServices.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentProductionItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    ProductionServices.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = ProductionServices.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa thiết bị sản xuất
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    ProductionServices.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<TreeStore>("StoreEquipmentProdutionID").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết thiết bị sản xuất
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentProductionItem();
            if (id != 0)
            {
                data = ProductionServices.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Phòng ban quản lý thiết bị
        [BaseSystem(Name = "Danh sách phòng ban quản lý")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DepartmentEquipmentForm(int id = 0)
        {
            var data = new EquipmentProductionDepartmentItem();
            data.EquipmentProductionID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DepartmentEquipment", Model = data };
        }
        #endregion

        #region Bảo dưỡng thiết bị sản xuất
        [BaseSystem(Name = "Bảo dưỡng")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult MaintainForm(int id = 0)
        {
            var data = new EquipmentProductionMaintainItem();
            data.EquipmentProductionID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Maintain", Model = data };
        }
        #endregion

        #region Khắc phục sự cố cho thiết bị sản xuất
        [BaseSystem(Name = "Khắc phục sự cố")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ErrorForm(int id = 0)
        {
            var data = new EquipmentProductionErrorItem();
            data.EquipmentProductionID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Error", Model = data };
        }
        #endregion

        #region Theo dõi thiết bị sản xuất
        [BaseSystem(Name = "Theo dõi")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult HistoryForm(int id = 0)
        {
            var data = new EquipmentProductionHistoryItem();
            data.EquipmentProductionID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "History", Model = data };
        }
        #endregion

        #region Dùng chung
        public ActionResult EquipmentProductionWindow()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "EquipmentProductionWindow" };
        }
        #endregion
    }
}
