using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Problem.Models;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Problem.Controllers
{
    [BaseSystem(Name = "Danh sách sự cố", IsActive = true, Icon = "Wrench", IsShow = true, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProblemEventTempController : BaseController
    {
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private ProblemEventAPI api = new ProblemEventAPI();
        private ProblemEventReportAPI apiRpt = new ProblemEventReportAPI();

        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }

        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult InsertWindow()
        {
            // Gán giá trị mặc định
            ProblemEventDTO objProblem = new ProblemEventDTO();

            // gán kiểu mặc định
            List<ProblemTypeDTO> lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;

            return new Ext.Net.MVC.PartialViewResult
            {
                Model = objProblem,
                ViewData = ViewData
            };
        }

        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult InsertWindowFromProblemEvent(int ID)
        {
            // Gán giá trị mặc định
            ProblemEventDTO objProblem = new ProblemEventDTO();
            // gán kiểu mặc định
            List<ProblemTypeDTO> lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;
            objProblem = api.GetByID(ID);
            objProblem.Code = "";
            ViewData["createFromProblemEvent"] = true;

            return new Ext.Net.MVC.PartialViewResult
            {
                Model = objProblem,
                ViewName = "InsertWindow",
                ViewData = ViewData
            };
        }

        [BaseSystem(Name = "Cập nhật", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateWindow(int ID)
        {
            ProblemEventDTO objProblem = new ProblemEventDTO();
            // gán kiểu mặc định
            List<ProblemTypeDTO> lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;
            objProblem = api.GetByID(ID);

            ViewData["ReadOnly"] = false;

            return new Ext.Net.MVC.PartialViewResult
            {
                Model = objProblem,
                ViewData = ViewData
            };
        }

        [BaseSystem(Name = "Xem", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailWindow(int ID)
        {
            ProblemEventDTO objProblem = new ProblemEventDTO();
            // gán kiểu mặc định
            List<ProblemTypeDTO> lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;
            objProblem = api.GetByID(ID);

            ViewData["ReadOnly"] = true;

            return new Ext.Net.MVC.PartialViewResult
            {
                Model = objProblem,
                ViewData = ViewData,
                ViewName = "UpdateWindow"
            };
        }

        public ActionResult GetData(StoreRequestParameters parameters, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, string choise = null, int? type = null, int? group = null)
        {
            var resp = api.GetData(parameters.Page, parameters.Limit, true, fromDate, toDate, type, group);
            int total = 0;
            if (resp.TotalRows != null)
                total = resp.TotalRows.Value;
            return this.Store(new Paging<ProblemEventListDTO>(resp.Data, total));
        }

        public async Task<ActionResult> GetTypeCbo()
        {
            var result = await api.GetTypeCbo();
            return this.Store(result);
        }
        public async Task<ActionResult> GetGroupCbo(int typeID = 0)
        {
            var result = await api.GetGroupCbo(typeID);
            return this.Store(result);
        }
        public async Task<ActionResult> GetEmergencyCbo()
        {
            var result = await api.GetEmergencyCbo();
            return this.Store(result);
        }
        public async Task<ActionResult> GetStatusCbo()
        {
            var result = await api.GetStatusCbo();
            return this.Store(result);
        }

        public async Task<ActionResult> GetCriticalLevelCbo()
        {
            var result = await api.GetCriticalLevelCbo();
            return this.Store(result);
        }

        public ActionResult Insert(ProblemEventDTO objNew, bool createFromProblemEvent = false)
        {
            bool success = false;
            try
            {
                // Nếu tạo mẫu từ sự cố
                if (createFromProblemEvent)
                    objNew.ID = 0;

                bool isUpdate = objNew.ID > 0;
                ResponseModel<object> result = api.InsertUpdate(objNew, User.ID, true);
                if (result.MessageCode != null && result.MessageCode.Equals("0000"))
                {
                    success = true;
                    if(isUpdate)
                        Ultility.ShowNotification(message: RequestMessage.UpdateSuccess);
                    else
                        Ultility.ShowNotification(message: RequestMessage.CreateSuccess);
                }
                else
                    Ultility.ShowNotification(message: RequestMessage.Error);
            }
            catch
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.FormPanel(success);
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                api.Delete(ID);
                X.GetCmp<Store>("stProblem").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }
        }

        public ActionResult GetReportList(int problemEventID)
        {
            var resp = apiRpt.GetData(problemEventID);
            int total = 0;
            if (resp.Data != null && resp.Data.Count > 0)
                total = resp.Data.Count;

            return this.Store(new Paging<ProblemEventReportDTO>(resp.Data, total));
        }

        public ActionResult InsertReportForm(int problemEventID)
        {
            var data = new ProblemEventReportInsModel();
            data.ProblemEventID = problemEventID;
            data.Status = 1;

            return new Ext.Net.MVC.PartialViewResult { Model = data };
        }

        public ActionResult UpdateReportForm(int ID)
        {
            ViewData["ReadOnly"] = false;
            var data = apiRpt.GetByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertReportForm", Model = data, ViewData = ViewData };
        }
        public ActionResult ViewReportForm(int ID)
        {
            ViewData["ReadOnly"] = true;
            var data = apiRpt.GetByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertReportForm", Model = data, ViewData = ViewData };
        }

        public ActionResult InsertUpdateReport(ProblemEventReportInsModel data)
        {
            try
            {
                apiRpt.InsertUpdate(data, User.ID);
                X.GetCmp<Store>("stReport").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }

        public ActionResult DeleteReport(int ID)
        {
            try
            {
                apiRpt.Delete(ID, User.ID);
                X.GetCmp<Store>("stReport").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }
    }
}