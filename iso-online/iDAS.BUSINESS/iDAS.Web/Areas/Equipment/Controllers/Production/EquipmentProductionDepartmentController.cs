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
    [BaseSystem(Name = "Cấp phát, thu hồi thiết bị sản xuất", IsActive = true, IsShow = false)]
    public class EquipmentProductionDepartmentsController : BaseController
    {
        private EquipmentProductionDepartmentSV ProductionDepartmentSV = new EquipmentProductionDepartmentSV();
        public ActionResult LoadDepartmentByProduct(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentProductionDepartmentItem>();
            if (id != 0)
            {
                result = ProductionDepartmentSV.GetByEquipmentProduction(parameters.Page, parameters.Limit, out totalCount, id);
            }
            return this.Store(result, totalCount);
        }
        #region Cập nhật Cấp phát thiết bị
        [BaseSystem(Name = "Cấp phát thiết bị")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int productionId = 0, int id = 0)
        {
            var data = new EquipmentProductionDepartmentItem();
            if (id != 0)
            {
                data = ProductionDepartmentSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                if (ProductionDepartmentSV.ExitsDepartmentUsing(productionId))
                {
                    Ultility.CreateMessageBox("Thông báo", "Thiết bị chưa được thu hồi");
                    return this.Direct();
                }
                data.ActionForm = Form.Add;
                data.EquipmentProductionID = productionId;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentProductionDepartmentItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    ProductionDepartmentSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = ProductionDepartmentSV.Insert(data, User.ID);
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

        #region Xóa Cấp phát thiết bị
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    ProductionDepartmentSV.Delete(id);
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

        #region Xem chi tiết Cấp phát thiết bị
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentProductionDepartmentItem();
            if (id != 0)
            {
                data = ProductionDepartmentSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Cập nhật Cấp phát thiết bị
        [BaseSystem(Name = "Thu hồi thiết bị")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Destroy(int productionId)
        {
            try
            {
                if (productionId != 0)
                {
                    ProductionDepartmentSV.Destroy(productionId, User.ID);
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
