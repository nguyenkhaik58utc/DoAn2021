using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items.Problem;
using iDAS.Utilities;
using iDAS.Web.API.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Problem.Controllers
{
    public class ProblemCriticalLevelController : Controller
    {
        ProblemCriticalLevelAPI api = new ProblemCriticalLevelAPI();
        // GET: Problem/ProblemType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetAll(parameters.Page, parameters.Limit).Result;
                List<ProblemCriticalLevelItem> data = resp.lstProblemCriticalLevel;
                foreach (ProblemCriticalLevelItem item in data)
                {
                    if (item.IsDefault)
                        item.textDefault = "Mặc định";
                    else
                        item.textDefault = "";
                }
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
        public ActionResult Create(ProblemCriticalLevelItem problemCriticalLevelItem)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    api.Create(problemCriticalLevelItem);
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
        public ActionResult Update(int ProblemCriticalLevelId)
        {
            try
            {
                var resp = api.GetById(ProblemCriticalLevelId).Result;
                var data = resp.lstProblemCriticalLevel[0];
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(ProblemCriticalLevelItem problemCriticalLevelItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Update(problemCriticalLevelItem);
                    X.GetCmp<Store>("StoreProblemCriticalLevel").Reload();
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
        public ActionResult Delete(int ProblemCriticalLevelId = 0)
        {
            try
            {
                var delete = api.delete(ProblemCriticalLevelId);
                if (delete.Result != 0)
                    Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
                else
                {
                    Ultility.ShowMessageBox("Không thành công","Mức độ nghiêm trọng đang được sử dụng",MessageBox.Icon.ERROR);
                }
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.DeleteError);
            }
            return this.Direct();
        }
        #endregion


        #region Default
        [BaseSystem(Name = "Mặc định")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Default()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Default(int ProblemCriticalLevelId = 0)
        {
            try
            {
                var defaulCritical = api.Default(ProblemCriticalLevelId);
                Ultility.ShowMessageRequest(RequestStatus.UpdateSuccess);
                return this.Direct();
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.UpdateSuccess);
            }
            return this.Direct();
        }
        #endregion

        #region Detail
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Detail(int ProblemCriticalLevelId)
        {
            try
            {
                var resp = api.GetById(ProblemCriticalLevelId).Result;
                var data = resp.lstProblemCriticalLevel[0];
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