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
    public class ProblemStatusController : Controller
    {
        ProblemStatusAPI api = new ProblemStatusAPI();
        // GET: Problem/ProblemEmergency
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetData(parameters.Page, parameters.Limit);
                List<ProblemStatusItem> lst = resp.Data;
                foreach (ProblemStatusItem item in lst)
                {
                    if (item.IsDefault)
                        item.textDefault = "Mặc định";
                    else
                        item.textDefault = "";
                }
                return this.Store(new Paging<ProblemStatusItem>(lst, resp.TotalRows.Value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult ShowFrmDetail(int ID)
        {
            var obj = api.GetByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }


        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
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
        public ActionResult Create(ProblemStatusItem obj)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    api.Insert(obj);
                    X.GetCmp<Store>("StoreProblemStatus").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
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


        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Update(int ID)
        {
            try
            {
                var obj = api.GetByID(ID).Result;
                return new Ext.Net.MVC.PartialViewResult() { Model = obj };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [HttpPost]
        public ActionResult Update(ProblemStatusItem objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Update(objEdit);
                    X.GetCmp<Store>("StoreProblemStatus").Reload();
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

        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            try
            {
                var delete  = api.Delete(ID);
                X.GetCmp<Store>("StoreProblemStatus").Reload();
                if (delete.Result != 0)
                    Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
                else
                {
                    Ultility.ShowMessageBox("Không thành công", "Trạng thái đang được sử dụng", MessageBox.Icon.ERROR);
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }
        }

        #region Delete
        [BaseSystem(Name = "Mặc định")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Default()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Default(int ProblemStatusId = 0)
        {
            try
            {
                var defaultStatus = api.Default(ProblemStatusId);
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
    }
}