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
    [BaseSystem(Name = "Sự cố thiết bị", IsActive = true, IsShow = false)]
    public class EquipmentProductionErrorController : BaseController
    {
        private EquipmentProductionErrorSV ErrorSV = new EquipmentProductionErrorSV();

        public ActionResult LoadErrorByProduct(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentProductionErrorItem>();
            if (id != 0)
            {
                result = ErrorSV.GetByEquipmentProduction(parameters.Page, parameters.Limit, out totalCount, id);
            }
            return this.Store(result, totalCount);
        }

        #region Cập nhật thiết bị đi kèm
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int productionId=0, int id = 0)
        {
            var data = new EquipmentProductionErrorItem();
            if (id != 0)
            {
                data = ErrorSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.EquipmentProductionID = productionId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentProductionErrorItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    ErrorSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = ErrorSV.Insert(data, User.ID);
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

        #region Xóa thiết bị đi kèm
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    ErrorSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<TreeStore>("StoreError").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết thiết bị đi kèm
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentProductionErrorItem();
            if (id != 0)
            {
                data = ErrorSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
