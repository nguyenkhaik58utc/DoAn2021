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
    [BaseSystem(Name = "Kết quả hiệu chỉnh thiết bị", IsActive = true, IsShow = false)]
    public class EquipmentMeasureCalibrationResultController : BaseController
    {
        private EquipmentMeasureCalibrationResultSV CalibrationResultSV = new EquipmentMeasureCalibrationResultSV();

        public ActionResult LoadCalibrationResult(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentMeasureCalibrationResultItem>();
            if (id != 0)
            {
                result = CalibrationResultSV.GetByMeasureCalibration(parameters.Page, parameters.Limit, out totalCount, id);
            }
            return this.Store(result, totalCount);
        }

        #region Cập nhật phân phối thiết bị
        [BaseSystem(Name = "Thêm và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int calibrationId = 0, int id = 0)
        {
            var data = new EquipmentMeasureCalibrationResultItem();
            if (id != 0)
            {
                data = CalibrationResultSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
                data.EquipmentMeasureCalibrationID = calibrationId;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentMeasureCalibrationResultItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    CalibrationResultSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = CalibrationResultSV.Insert(data, User.ID);
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
                    CalibrationResultSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<Store>("StoreCalibrationResult").Reload();
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
            var data = new EquipmentMeasureCalibrationResultItem();
            if (id != 0)
            {
                data = CalibrationResultSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
