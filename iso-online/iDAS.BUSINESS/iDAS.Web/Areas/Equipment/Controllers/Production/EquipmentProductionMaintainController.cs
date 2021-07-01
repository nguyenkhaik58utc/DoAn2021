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
    [BaseSystem(Name = "Bảo dưỡng thiết bị sản xuất", IsActive = true, IsShow = false)]
    public class EquipmentProductionMaintainController : BaseController
    {
        private EquipmentProductionMaintainSV MaintainSV = new EquipmentProductionMaintainSV();

        public ActionResult LoadMaintainByProduct(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentProductionMaintainItem>();
            if (id != 0)
            {
                result = MaintainSV.GetByEquipmentProduction(parameters.Page, parameters.Limit, out totalCount, id);
            }
            return this.Store(result, totalCount);
        }

        #region Cập nhật bảo dưỡng thiết bị
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int productionId=0, int id = 0)
        {
            var data = new EquipmentProductionMaintainItem();
            if (id != 0)
            {
                data = MaintainSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.EquipmentProductionID = productionId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentProductionMaintainItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    MaintainSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = MaintainSV.Insert(data, User.ID);
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

        #region Cập nhật kết quả thực tế
        [BaseSystem(Name = "Kết quả thực tế")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateRealForm(int id = 0)
        {
            var data = new EquipmentProductionMaintainItem();
            if (id != 0)
            {
                data = MaintainSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateReal", Model = data };
        }
        public ActionResult UpdateReal(EquipmentProductionMaintainItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    MaintainSV.UpdateReal(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa bảo dưỡng thiết bị
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    MaintainSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<TreeStore>("StoreMaintain").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết bảo dưỡng thiết bị
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentProductionMaintainItem();
            if (id != 0)
            {
                data = MaintainSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
