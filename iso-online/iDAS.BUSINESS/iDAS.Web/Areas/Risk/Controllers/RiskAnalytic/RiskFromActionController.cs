using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;


namespace iDAS.Web.Areas.Risk.Controllers
{

    [BaseSystem(Name = "Rủi ro từ quy trình hoạt động", Icon = "ApplicationViewList", IsActive = true, IsShow = true, Position = 4, Parent = RiskAnalyticController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskFromActionController : BaseController
    {
        private RiskSV RiskSV = new RiskSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int departmentId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = RiskSV.GetRiskFromActionByDepartment(filter, departmentId: departmentId);
            return this.Store(result, filter.PageTotal);
        }

        public ActionResult LoadRiskCenter(StoreRequestParameters parameters, int cateId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = (new CenterRiskSV()).GetByCategory(filter, cateId);
            return this.Store(result, filter.PageTotal);
        }

        public ActionResult LoadTreatment(StoreRequestParameters parameters, int riskId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = (new CenterRiskSV()).GetTreatment(filter, riskId);
            return this.Store(result, filter.PageTotal);
        }

        #region Thêm mới và sửa
        [BaseSystem(Name = "Thêm và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int departId = 0)
        {
            var data = new RiskItem();
            if (id != 0)
            {
                data = RiskSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                if (departId != 0)
                {
                    data.Department = new HumanDepartmentViewItem
                    {
                        ID = departId,
                        Name = new DepartmentSV().GetByID(departId).Name
                    };
                }
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskItem data)
        {
            try
            {
                if (data.ID == 0)
                {
                    RiskSV.InsertRiskFromAction(data, User.ID);
                    X.GetCmp<Store>("StoreRisk").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    RiskSV.UpdateRiskFromAction(data, User.ID);
                    X.GetCmp<Store>("StoreRisk").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    RiskSV.Delete(id);
                    X.GetCmp<Store>("StoreRisk").Reload();
                    Ultility.CreateNotification(title: RequestMessage.Notify, message: RequestMessage.DeleteSuccess);
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
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0, int departId = 0)
        {
            var data = new RiskItem();
            data = RiskSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Phân tích rủi ro
        [BaseSystem(Name = "Phân tích rủi ro")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Analytic(int id)
        {
            var data = new RiskItem();
            if (id != 0)
            {
                data = RiskSV.GetAnalytic(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Analytic", Model = data };
        }
        [HttpPost]
        public ActionResult Analytic(RiskItem data)
        {
            try
            {
                if (RiskSV.Analytic(data, User.ID))
                {
                    X.GetCmp<Store>("StoreRisk").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    Ultility.CreateMessageBox(title: RequestMessage.Notify, message: "Phân tích này đang kiểm soát");
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Gửi rủi ro để kiểm soát
        [BaseSystem(Name = "Gửi kiểm soát")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult SendRisk(int id)
        {
            try
            {
                if (id != 0)
                {
                    RiskSV.SendRisk(id, User.ID);
                    X.GetCmp<Store>("StoreRisk").Reload();
                    Ultility.CreateNotification(message: RequestMessage.SendSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.Direct();
        }
        #endregion

    }
}
