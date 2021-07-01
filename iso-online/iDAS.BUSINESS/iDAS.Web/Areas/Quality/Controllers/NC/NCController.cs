using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Quality.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý sự không phù hợp", Icon = "AsteriskYellow", IsActive = true, IsShow = true, Position = 6)]
    public class NCController : BaseController
    {
        private QualityNCSV NCSV = new QualityNCSV();
        private QualityCAPARequirementSV CAPARequireSV = new QualityCAPARequirementSV();
        private ISOStandartCriteriaSV iSOStandartCriteriaSV = new ISOStandartCriteriaSV();
        #region Xem danh sách
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListByIndex(string urlSubmit = "", string urlStore = "", StoreParameter paramStore = null)
        {
            ViewData["SubmitUrl"] = urlSubmit;
            ViewData["StoreUrl"] = urlStore;
            ViewData["StoreParam"] = paramStore == null ? new StoreParameter() : paramStore;

            return PartialView("List");
        }
        public ActionResult LoadNCs(StoreRequestParameters parameters, int departmentId = 0)
        {
            int totalCount;
            var model = NCSV.GetByDepartment(parameters.Page, parameters.Limit, out totalCount, departmentId);
            return this.Store(model, totalCount);
        }
        #endregion

        #region Thêm
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới")]
        public ActionResult InsertView(string urlSubmit = "", Parameter param = null)
        {
            var model = new QualityNCItem()
            {
                Enums = new TypeModel()
                {
                    Type = EType.Obs,
                }
            };
            ViewData["UrlSubmit"] = urlSubmit;
            ViewData["Parameter"] = param;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName="InsertView",
                ViewData = ViewData,
                Model = model
            };
        }
        public ActionResult Insert(QualityNCItem item)
        {
            var success = false;
            try
            {

                item.CreateBy = User.ID;
                NCSV.Insert(item);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }
        public ActionResult LoadEmployee(StoreRequestParameters parameters, string lsEmployeeId)
        {
            if (!string.IsNullOrEmpty(lsEmployeeId))
            {
                int totalCount;
                var employeeSV = new HumanEmployeeSV();
                var ids = lsEmployeeId.Split(',').Select(n => int.Parse(n)).ToArray();
                var model = employeeSV.GetEmployeesByIDs(parameters.Page, parameters.Limit, out totalCount, ids);
                return this.Store(model, totalCount);
            }
            return null;
        }
        public ActionResult LoadRole(StoreRequestParameters parameters, string lsRoleId)
        {
            if (!string.IsNullOrEmpty(lsRoleId))
            {
                int totalCount;
                var roleSV = new RoleSV();
                var ids = lsRoleId.Split(',').Select(n => int.Parse(n)).ToArray();
                var model = roleSV.GetByIds(parameters.Page, parameters.Limit, out totalCount, ids);
                return this.Store(model, totalCount);
            }
            return null;
        }
        public ActionResult NCRole()
        {
            return new Ext.Net.MVC.PartialViewResult();

        }
        public ActionResult NCEmployee()
        {
            return new Ext.Net.MVC.PartialViewResult();
        }
        #endregion

        #region Sửa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        public ActionResult UpdateView(int id)
        {
            var model = NCSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
                Model = model,
            };
        }
        public ActionResult Update(QualityNCItem item)
        {
            bool success = false;
            try
            {
                NCSV.Update(item, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Xóa
        /// <summary>
        /// Xóa kế hoạch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            var success = false;
            try
            {

                NCSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: "Dữ liệu đang được sử dụng", error: true);
                return this.Direct();
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Xem chi tiết
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailView(int id)
        {
            var model = NCSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
                Model = model,
            };
        }
        #endregion

        #region Dùng chung
        public ActionResult ListWindow(string urlStore = "", int paramID = 0)
        {
            ViewData["UrlStore"] = urlStore;
            ViewData["ParamID"] = paramID;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        public ActionResult List(string containerId, string urlSubmit = "", string urlStore = "", StoreParameter paramStore = null)
        {
            ViewData["SubmitUrl"] = urlSubmit;
            ViewData["StoreUrl"] = urlStore;
            ViewData["StoreParam"] = paramStore == null ? new StoreParameter() : paramStore;

            return new Ext.Net.MVC.PartialViewResult
               {
                   ViewData = ViewData,
                   RenderMode = RenderMode.AddTo,
                   ContainerId = containerId,
                   WrapByScriptTag = false,
               };
        }

        #region Dùng chung đánh giá chung
        public ActionResult NCShow(int auditId = 0, int criteriaId = 0, int departmentId = 1)
        {
            var data = new QualityNCItem();
            data = NCSV.GetByAudit(auditId, criteriaId, departmentId);
            ViewData["auditId"] = auditId;
            ViewData["qualityCriteriaID"] = criteriaId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = data.ID != 0 ? (data.IsVerify ? "DetailView" : "UpdateView") : "NCCommon",
                ViewData = ViewData,
                Model = data,
            };
        }
        #endregion
        #region Dùng chung đánh giá nội bộ
        public ActionResult NCShowQuality(int auditId = 0, int criteriaId = 0, int departmentId = 0)
        {
            var data = new QualityNCItem();
            data = NCSV.GetByAuditQuality(auditId, criteriaId, departmentId);
            ViewData["auditId"] = auditId;
            ViewData["qualityCriteriaID"] = criteriaId;
            ViewData["qualityCriteriaName"] = iSOStandartCriteriaSV.GetByID(criteriaId).Name;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = data.ID != 0 ? (data.IsVerify ? "DetailView" : "UpdateView") : "NCQuality",
                ViewData = ViewData,
                Model = data,
            };
        }
        public ActionResult NCShowDetailQuality(int auditId = 0, int criteriaId = 0, int departmentId = 0)
        {
            var data = new QualityNCItem();
            data = NCSV.GetByAuditQuality(auditId, criteriaId, departmentId);
            ViewData["auditId"] = auditId;
            ViewData["qualityCriteriaID"] = criteriaId;
            ViewData["qualityCriteriaName"] = iSOStandartCriteriaSV.GetByID(criteriaId).Name;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "DetailView",
                ViewData = ViewData,
                Model = data,
            };
        }
        #endregion
        #region Dùng chung đánh giá rui ro
        public ActionResult NCShowRiskAudit(int auditId = 0, int RiskControlID = 0, int departmentId = 0)
        {
            var data = new QualityNCItem();
            data = NCSV.GetByAuditRiskControl(auditId, RiskControlID, departmentId);
            ViewData["auditId"] = auditId;
            ViewData["RiskControlID"] = RiskControlID;        
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = data.ID != 0 ? (data.IsVerify ? "DetailView" : "UpdateView") : "NCAuditRiskControl",
                ViewData = ViewData,
                Model = data,
            };
        }
        #endregion
        #endregion

        #region Yêu cầu khắc phục phòng ngừa
        public ActionResult CAPARequire(int id)
        {
            var data = new QualityCAPARequireItem() { NCID = id };
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "CAPARequire",
                Model = data,
            };
        }
        public ActionResult LoadCAPARequire(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount;
            var result = CAPARequireSV.GetByNC(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateCAPARequireForm(int id = 0, int NCId = 0)
        {
            var data = new QualityCAPARequireItem()
            {
                NCID = NCId
            };
            if (id != 0)
            {
                data = CAPARequireSV.GetByID(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateCAPARequire", Model = data };
        }
        public ActionResult UpdateRequire(QualityCAPARequireItem data)
        {
            bool success = false;
            try
            {
                if (data.ID == 0)
                {
                    data.CreateBy = User.ID;
                    CAPARequireSV.Insert(data);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    success = true;
                }
                else
                {
                    data.UpdateBy = User.ID;
                    CAPARequireSV.Update(data);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.FormPanel(success);
        }
        public ActionResult DeleteCAPARequire(int id)
        {
            try
            {
                CAPARequireSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);

            }
            return this.Direct();
        }
        public ActionResult DeltailCAPARequireForm(int id = 0)
        {
            var data = new QualityCAPARequireItem();
            if (id != 0)
            {
                data = CAPARequireSV.GetByID(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailCAPARequire", Model = data };
        }
        #endregion

        #region Xác nhận
        public ActionResult VerifyView(int id)
        {
            var model = NCSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "VerifyView",
                Model = model,
            };
        }
        public ActionResult Verify(QualityNCItem item)
        {
            bool success = false;
            try
            {
                item.IsVerify = true;
                NCSV.Verify(item, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }

        #endregion
    }
}
