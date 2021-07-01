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
    [BaseSystem(Name = "Hiệu chuẩn thiết bị", IsActive = true, IsShow = false)]
    public class EquipmentMeasureCalibrationController : BaseController
    {
        private EquipmentMeasureCalibrationSV CalibrationSV = new EquipmentMeasureCalibrationSV();

        public ActionResult LoadCalibrationByMeasure(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentMeasureCalibrationItem>();
            if (id != 0)
            {
                result = CalibrationSV.GetByEquipmentMeasure(parameters.Page, parameters.Limit, out totalCount, id);
            }
            return this.Store(result, totalCount);
        }

        #region Cập nhật phân phối thiết bị
        [BaseSystem(Name = "Thêm và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int measureId = 0, int id = 0)
        {
            var data = new EquipmentMeasureCalibrationItem();
            if (id != 0)
            {
                data = CalibrationSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
                data.EquipmentMeasureID = measureId;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentMeasureCalibrationItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    CalibrationSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = CalibrationSV.Insert(data, User.ID);
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

        #region Xóa phân phối thiết bị
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    CalibrationSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<Store>("StoreMeasureCalibration").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết phân phối thiết bị
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentMeasureCalibrationItem();
            if (id != 0)
            {
                data = CalibrationSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Kết quả phân phối thiết bị
        [BaseSystem(Name = "Cập nhật kết quả")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateResultForm(int id = 0)
        {
            var data = new EquipmentMeasureCalibrationItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateResult", Model = data };
        }
        #endregion

        #region Danh sách thiết bị hiệu chỉnh
        public ActionResult LoadCalibration()
        {
            int totalCount = 0;
             var   result = CalibrationSV.GetCalibration();
            return this.Store(result, totalCount);
        }
        #endregion

    }
}
