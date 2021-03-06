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
    [BaseSystem(Name = "Cấp phát, thu hồi thiết bị đo", IsActive = true, IsShow = false)]
    public class EquipmentMeasureDepartmentController : BaseController
    {
        private EquipmentMeasureDepartmentSV DepartmentSV = new EquipmentMeasureDepartmentSV();

        public ActionResult LoadDepartmentByProduct(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentMeasureDepartmentItem>();
            if (id != 0)
            {
                result = DepartmentSV.GetByEquipmentMeasure(parameters.Page, parameters.Limit, out totalCount, id);
            }
            return this.Store(result, totalCount);
        }

        #region Cập nhật phân phối thiết bị
        [BaseSystem(Name = "Cấp phát thiết bị")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int measureId = 0, int id = 0)
        {
            var data = new EquipmentMeasureDepartmentItem();
            if (id != 0)
            {
                data = DepartmentSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                if (DepartmentSV.ExitsDepartmentUsing(measureId))
                {
                    Ultility.CreateMessageBox("Thông báo", "Thiết bị chưa được thu hồi");
                    return this.Direct();
                }
                data.ActionForm = Form.Add;
                data.EquipmentMeasureID = measureId;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentMeasureDepartmentItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    DepartmentSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = DepartmentSV.Insert(data, User.ID);
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
                    DepartmentSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<Store>("StoreDepartmentEquipment").Reload();
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
            var data = new EquipmentMeasureDepartmentItem();
            if (id != 0)
            {
                data = DepartmentSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Cập nhật phân phối thiết bị
        [BaseSystem(Name = "Thu hồi thiết bị")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Destroy(int measureId)
        {
            try
            {
                if (measureId != 0)
                {
                    DepartmentSV.Destroy(measureId, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>("StoreDepartmentEquipment").Reload();
                }
                else Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion
    }
}
