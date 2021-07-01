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

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Đợt đào tạo")]
    public class PlanDetailController : BaseController
    {
        private HumanTrainingPlanDetailSV planDetailService = new HumanTrainingPlanDetailSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int planId=0)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewData = new ViewDataDictionary { { "planId", planId } } };
        }
        public ActionResult GetData(StoreRequestParameters parameters, int planId)
        {
            List<HumanTrainingPlanDetailItem> lstData;
            int total;
            lstData = planDetailService.GetAll(parameters.Page, parameters.Limit, out total, planId);
            return this.Store(new Paging<HumanTrainingPlanDetailItem>(lstData, total));
        }
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd(int planId)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Insert", ViewData = new ViewDataDictionary { { "planId", planId } } };
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id)
        {
            var obj = planDetailService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Hủy")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmCancel(int id)
        {
            var obj = planDetailService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "FrmCancel", Model = obj };
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id)
        {
            var obj = planDetailService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        public ActionResult UpdateIsCancel(HumanTrainingPlanDetailItem objEdit)
        {
            try
            {
                planDetailService.UpdateCancel(objEdit, User.ID);
                X.GetCmp<Store>("stMnPlanDetail").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
       [ValidateInput(false)]
        public ActionResult Insert(HumanTrainingPlanDetailItem objNew)
        {

            try
            {
                planDetailService.Insert(objNew, User.ID);               
                X.GetCmp<Store>("stMnPlanTrainingDetail").Reload();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();

        }
       [ValidateInput(false)]       
        public ActionResult Update(HumanTrainingPlanDetailItem objEdit)
        {

            try
            {
                planDetailService.Update(objEdit, User.ID);              
                X.GetCmp<Store>("stMnPlanTrainingDetail").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                planDetailService.Delete(id);
                X.GetCmp<Store>("stMnPlanTrainingDetail").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                throw;
            }
            return this.Direct();
        }
    }
}
