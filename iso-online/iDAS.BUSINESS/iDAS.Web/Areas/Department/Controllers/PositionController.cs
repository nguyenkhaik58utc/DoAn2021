using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Items.Position;
using iDAS.Web.API.ManagementLevel;

namespace iDAS.Web.Areas.Department.Controllers
{
    public class PositionController : Controller
    {
        PositionAPI api = new PositionAPI();
        ManagementLevelAPI levelAPI = new ManagementLevelAPI();
        // GET: Department/Oganization
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetPositionAPI().Result;
                var data = resp.lstPosition;
                total = resp.total;
                return this.Store(data, total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetAllManagementLevel(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = levelAPI.GetManagementLevelAPI(parameters.Page, parameters.Limit).Result;
                var data = resp.lstManagementItem;
                total = resp.total;
                return this.Store(data, total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Create
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Create()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(PositionItem position)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    api.Create(position);
                    Ultility.ShowMessageRequest(RequestStatus.CreateSuccess);
                    success = true;
                }
                else
                {
                    Ultility.ShowMessageRequest(RequestStatus.ValidError);
                    success = false;
                }
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Update
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Update(int PositionId)
        {
            try
            {
                var resp = api.GetPositionById(PositionId).Result;
                var data = resp.lstPosition[0];
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(PositionItem position)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Update(position);
                    X.GetCmp<Store>("StorePosition").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
        #endregion

        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int PositionId = 0)
        {
            try
            {
                var delete = api.delete(PositionId);
                Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.DeleteError);
            }
            return this.Direct();
        }
        #endregion

        #region Detail
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Detail(int positionId)
        {
            try
            {
                var resp = api.GetPositionById(positionId).Result;
                var data = resp.lstPosition[0];
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
    }
}