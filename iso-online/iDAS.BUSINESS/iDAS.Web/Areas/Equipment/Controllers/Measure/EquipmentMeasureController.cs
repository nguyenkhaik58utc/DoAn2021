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

namespace iDAS.Web.Areas.Equipment.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thiết Bị Đo Lường", Icon = "Table", IsActive = true, IsShow = true, Position = 3)]
    public class EquipmentMeasureController : BaseController
    {
        private EquipmentMeasureSV equipmentMeasureSV = new EquipmentMeasureSV();
        private EquipmentMeasureCategorySV equipmentMeasureCategorySV = new EquipmentMeasureCategorySV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadMeasure(StoreRequestParameters parameters, int categoryId = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentMeasureItem>();
            if (categoryId != 0)
            {
                result = equipmentMeasureSV.GetAll(parameters.Page, parameters.Limit, out totalCount, categoryId);
            }
            return this.Store(result, totalCount);
        }

        #region Cập nhật thiết bị đo lường
        [BaseSystem(Name = "Thêm mới và sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0, int cateId = 0)
        {
            var data = new EquipmentMeasureItem();
            if (id != 0)
            {
                data = equipmentMeasureSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.EquipmentMeasureCategoryName = equipmentMeasureCategorySV.GetById(cateId).Name;
                data.EquipmentMeasureCategoryID = cateId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentMeasureItem data)
        {
            int id = 0;
            try
            {
                if (equipmentMeasureSV.checkCodeExist(data.Code, data.ID))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Mã thiết bị đã tồn tại!"
                    });
                }
                else
                {
                    if (data.ID != 0)
                    {
                        equipmentMeasureSV.Update(data, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                        id = data.ID;
                    }
                    else
                    {

                        id = equipmentMeasureSV.Insert(data, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa thiết bị đo lường
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    equipmentMeasureSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<Store>("StoreEquipmentMeasure").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết thiết bị đo lường
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentMeasureItem();
            if (id != 0)
            {
                data = equipmentMeasureSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Phòng ban quản lý thiết bị đo lường
        [BaseSystem(Name = "Danh sách phòng ban quản lý")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DepartmentEquipmentForm(int id = 0, string name = "")
        {
            var data = new EquipmentMeasureItem();
            data.ID = id;
            data.Name = name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DepartmentEquipment", Model = data };
        }
        #endregion

        #region Kiểm định thiết bị
        [BaseSystem(Name = "Kiểm định")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult CalibrationForm(int id = 0, string name = "")
        {
            var data = new EquipmentMeasureItem();
            data.ID = id;
            data.Name = name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Calibration", Model = data };
        }
        #endregion

        #region Hiệu chỉnh thiết bị đo lường
        [BaseSystem(Name = "Hiệu chỉnh")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult MeasureCalibrationForm(int id = 0, string name = "")
        {
            var data = new EquipmentMeasureItem();
            data.ID = id;
            data.Name = name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "MeasureCalibration", Model = data };
        }
        #endregion

        #region Dùng chung
        public ActionResult EquipmentMeasureWindow()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "EquipmentMeasureWindow" };
        }
        #endregion
    }
}
