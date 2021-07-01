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
    public class EquipmentProductionAttachController : BaseController
    {
        private EquipmentProductionAttachSV AttachSV = new EquipmentProductionAttachSV();

        public ActionResult LoadAttachByProduct(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount = 0;
            var result = new List<EquipmentProductionAttachItem>();
            if (id != 0)
            {
                result = AttachSV.GetByEquipmentProduction(parameters.Page, parameters.Limit, out totalCount, id);
            }
            return this.Store(result, totalCount);
        }

        #region Cập nhật thiết bị đi kèm
        public ActionResult UpdateForm(int productionId=0, int id = 0)
        {
            var data = new EquipmentProductionAttachItem();
            if (id != 0)
            {
                data = AttachSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.EquipmentProductionID = productionId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentProductionAttachItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    AttachSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = AttachSV.Insert(data, User.ID);
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
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    AttachSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    success = true;
                    X.GetCmp<TreeStore>("StoreEquipmentAttach").Reload();
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
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentProductionAttachItem();
            if (id != 0)
            {
                data = AttachSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
