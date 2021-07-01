using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items.Position;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.API.ManagementLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Department.Controllers
{
    public class ManagementLevelController : Controller
    {
        PositionAPI PositionAPI = new PositionAPI();
        ManagementLevelAPI api = new ManagementLevelAPI();
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
                var resp = api.GetManagementLevelAPI(parameters.Page, parameters.Limit).Result;
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
        public ActionResult Create(ManagementLevelItem managementLevel)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    api.Create(managementLevel);
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
        public ActionResult Update(int ManagementLevelId)
        {
            try
            {
                var resp = api.GetManagementLevelById(ManagementLevelId).Result;
                var data = resp.lstManagementItem[0];
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(ManagementLevelItem managementLevel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Update(managementLevel);
                    X.GetCmp<Store>("StoreManagementLevel").Reload();
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
        public ActionResult Delete(int ManagementLevelId = 0)
        {
            try
            {
                var lstPosition = PositionAPI.GetPositionAPI().Result.lstPosition;
                bool check = false;
                foreach (var position in lstPosition)
                {
                    if (position.ManagementLevelID == ManagementLevelId)
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                {
                    Ultility.ShowMessageRequest(RequestStatus.DeleteError);
                }
                else
                {
                    var delete = api.delete(ManagementLevelId);
                    Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
                }
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
        public ActionResult Detail(int ManagementLevelId)
        {
            try
            {
                var resp = api.GetManagementLevelById(ManagementLevelId).Result;
                var data = resp.lstManagementItem[0];
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