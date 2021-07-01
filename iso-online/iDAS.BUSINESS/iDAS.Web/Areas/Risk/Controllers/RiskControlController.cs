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
    [BaseSystem(Name = "Kiểm soát rủi ro", Icon = "Tick", IsActive = true, IsShow = true, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskControlController : BaseController
    {
        private RiskControlSV RiskControlSV = new RiskControlSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadRisk(StoreRequestParameters parameters, int departmentId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = RiskControlSV.GetRisk(filter,departmentId);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int riskId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = RiskControlSV.GetByRisk(filter, RiskID: riskId);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult LoadTempSolution(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = (new RiskLibrarySolutionSV()).GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult LoadRiskSolution(StoreRequestParameters parameters, string lstSolutionTempId)
        {
            if (!string.IsNullOrEmpty(lstSolutionTempId))
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                var result = RiskControlSV.GetSolution(filter,lstSolutionTempId);
                return this.Store(result, filter.PageTotal);
            }
            return this.Store(null,0);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int riskId = 0, float impact = 0, int riskCategoryId = 0, int consequence=0)
        {
            var data = new RiskControlItem();
            if (id != 0)
            {
                data = RiskControlSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.Impact = impact;
                data.RiskCategoryID = riskCategoryId;
                data.NowConsequence = consequence;
                data.RiskID = riskId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskControlItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    RiskControlSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    RiskControlSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winUpdateRiskControl").Close();
                X.GetCmp<Store>("StoreRiskControl").Reload();
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
                    RiskControlSV.Delete(id);
                    X.GetCmp<Store>("StoreRiskControl").Reload();
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
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new RiskControlItem();
            if (id != 0)
            {
                data = RiskControlSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Phê duyệt
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Approve(int id)
        {
            var data = new RiskControlItem();
            if (id != 0)
            {
                data = RiskControlSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
        }
        [HttpPost]
        public ActionResult Approve(RiskControlItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    RiskControlSV.Approve(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winApproveRiskControl").Close();
                X.GetCmp<Store>("StoreRiskControl").Reload();
            }
            return this.Direct();
        }
        #endregion
    }
}
