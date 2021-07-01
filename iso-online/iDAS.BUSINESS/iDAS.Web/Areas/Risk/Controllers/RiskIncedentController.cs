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

namespace iDAS.Web.Areas.Risk.Controllers
{
    public class RiskIncedentController : BaseController
    {
        private RiskIncedentSV RiskIncedentSV = new RiskIncedentSV();
        public ActionResult Index(int riskId)
        {
            ViewData["ID"] = riskId;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Index" ,ViewData = ViewData};
        }
        public ActionResult LoadData(StoreRequestParameters parameters,int riskId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = RiskIncedentSV.GetByRisk(filter,riskId);
            return this.Store(result, filter.PageTotal);
        }
        #region Cập nhật
        [HttpGet]
        public ActionResult Update(int id = 0, int riskId = 0)
        {
            var data = new RiskIncedentItem();
            if (id != 0)
            {
                data = RiskIncedentSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.RiskID = riskId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskIncedentItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    RiskIncedentSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    RiskIncedentSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRiskIncedent").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    RiskIncedentSV.Delete(id);
                    X.GetCmp<Store>("StoreRiskIncedent").Reload();
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        public ActionResult DetailForm(int id = 0)
        {
            var data = new RiskIncedentItem();
            if (id != 0)
            {
                data = RiskIncedentSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

    }
}
