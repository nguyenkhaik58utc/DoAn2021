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
    [BaseSystem(Name = "Kiểm soát kỹ thuật", Icon = "TagBlue", IsActive = true, IsShow = true, Parent = RiskCategoryController.Code, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class RiskSolutionController : BaseController
    {
        private RiskLibrarySolutionSV RiskLibrarySolutionSV = new RiskLibrarySolutionSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadSolution(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = new ISOControlSV().GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int riskTempId)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = RiskLibrarySolutionSV.GetByRiskTemp(filter, riskTempId);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật
        [BaseSystem(Name = "Thêm và Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int riskTempId = 0)
        {
            var data = new RiskLibrarySolutionItem();
            if (id != 0)
            {
                data = RiskLibrarySolutionSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.RiskTempControlID = riskTempId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(RiskLibrarySolutionItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    RiskLibrarySolutionSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    RiskLibrarySolutionSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRiskLibrarySolution").Reload();
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
                    RiskLibrarySolutionSV.Delete(id);
                    X.GetCmp<Store>("StoreRiskLibrarySolution").Reload();
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
            var data = new RiskLibrarySolutionItem();
            if (id != 0)
            {
                data = RiskLibrarySolutionSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Xem chi tiết kiểm soát mẫu
        public ActionResult RiskTempDetail(int riskTempId = 0)
        {
            if (riskTempId != 0)
            {
                var data = new ISOControlSV().GetByID(riskTempId);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "TempDetail", Model = data };
            }
            return this.Direct();
        }
        #endregion

    }
}
