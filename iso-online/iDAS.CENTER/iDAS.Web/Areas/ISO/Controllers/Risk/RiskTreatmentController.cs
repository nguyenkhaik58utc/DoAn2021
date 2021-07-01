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

namespace iDAS.Web.Areas.ISO.Controllers
{
    public class RiskTreatmentController : BaseController
    {
        private RiskTreatmentSV RiskTreatmentSV = new RiskTreatmentSV();
        public ActionResult Treatment(int id)
        {
            var data = new RiskTreatmentItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Treatment", Model = data };
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int id)
        {
            if (id != 0)
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                var result = RiskTreatmentSV.GetByRisk(filter, id);
                return this.Store(result, filter.PageTotal);
            }
            return this.Store(null, 0);
        }
        public ActionResult FillColor()
        {
            var lst = iDAS.Utilities.GetData.RenderColor();
            return this.Store(lst);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int riskId = 0)
        {
            var data = new RiskTreatmentItem();
            if (id != 0)
            {
                data = RiskTreatmentSV.GetById(id);
                data.ActionForm = "Edit";
            }
            else
            {
                data.CenterRiskID = riskId;
                data.ActionForm = "Add";
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskTreatmentItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    RiskTreatmentSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
                else
                {
                    RiskTreatmentSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: Message.InsertSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRiskTreatment").Reload();
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
                    RiskTreatmentSV.Delete(id);
                    X.GetCmp<Store>("StoreRiskTreatment").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: Message.Notify, message: Message.DeleteError, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new RiskTreatmentItem();
            if (id != 0)
            {
                data = RiskTreatmentSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
