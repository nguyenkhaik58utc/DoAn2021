using Ext.Net;
using Ext.Net.MVC;
using iDAS.Base;
using iDAS.Core;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Problem.Models;
using iDAS.Web.Report;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace iDAS.Web.Areas.Problem.Controllers
{
    [BaseSystem(Name = "Danh sách sự cố", IsActive = true, Icon = "Wrench", IsShow = true, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProblemEventController : BaseController
    {
        private V3ProblemEventSV problemEventSV = new V3ProblemEventSV();
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        private ProblemEventAPI api = new ProblemEventAPI();
        private ProblemEventReportAPI apiRpt = new ProblemEventReportAPI();
        private HumanUserSV userSV = new HumanUserSV();

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
            objProblem.Receiver = User.EmployeeID;
            objProblem.ReceiverName = User.Name;

            // gán trạng thái mặc định
            List<ProblemStatusDTO> lstStatus = api.GetStatusCbo().Result;
            ViewData["lstStatus"] = lstStatus;
            var statusDefault = lstStatus.FirstOrDefault(x => x.IsDefault == true);
            if (statusDefault != null)
                objProblem.StatusID = statusDefault.ID;
            else if (lstStatus.Count > 0)
                objProblem.StatusID = lstStatus[0].ID;

            // gán kiểu mặc định
            List<ProblemTypeDTO>  lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemEmergencyDTO> lstEmergency = api.GetEmergencyCbo().Result;
            ViewData["lstEmergency"] = lstEmergency;
            var emergencyDefault = lstEmergency.FirstOrDefault(x => x.IsDefault == true);
            if (emergencyDefault != null)
                objProblem.EmergencyTypeID = emergencyDefault.ID;
            else if (lstEmergency.Count > 0)
                objProblem.EmergencyTypeID = lstEmergency[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemCriticalLevelDTO> lstCri = api.GetCriticalLevelCbo().Result;
            ViewData["lstCri"] = lstCri;
            var criDefault = lstCri.FirstOrDefault(x => x.IsDefault == true);
            if (criDefault != null)
                objProblem.CriticalLevelID = criDefault.ID;
            else if (lstCri.Count > 0)
                objProblem.CriticalLevelID = lstCri[0].ID;

            objProblem.ResidentAgencyGroupID = 0;
            objProblem.ResidentAgencyID = 0;
            objProblem.RequestDepID = 0;

            objProblem.OnDuty = true;
            objProblem.OnDutyText = "true";

            return new Ext.Net.MVC.PartialViewResult
            {
                Model = objProblem,
                ViewData = ViewData
            };
        }

        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult InsertWindowFromTemp(int ID)
        {
            // Gán giá trị mặc định
            ProblemEventDTO objProblem = new ProblemEventDTO();
            objProblem = api.GetByID(ID);
            objProblem.Receiver = User.EmployeeID;
            objProblem.ReceiverName = User.Name;
            objProblem.Code = string.Format("SC{0}", long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
            ViewData["createFromTemp"] = true;

            // gán trạng thái mặc định
            List<ProblemStatusDTO> lstStatus = api.GetStatusCbo().Result;
            ViewData["lstStatus"] = lstStatus;
            var statusDefault = lstStatus.FirstOrDefault(x => x.IsDefault == true);
            if (statusDefault != null)
                objProblem.StatusID = statusDefault.ID;
            else if (lstStatus.Count > 0)
                objProblem.StatusID = lstStatus[0].ID;

            // gán kiểu mặc định
            List<ProblemTypeDTO> lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemEmergencyDTO> lstEmergency = api.GetEmergencyCbo().Result;
            ViewData["lstEmergency"] = lstEmergency;
            var emergencyDefault = lstEmergency.FirstOrDefault(x => x.IsDefault == true);
            if (emergencyDefault != null)
                objProblem.EmergencyTypeID = emergencyDefault.ID;
            else if (lstEmergency.Count > 0)
                objProblem.EmergencyTypeID = lstEmergency[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemCriticalLevelDTO> lstCri = api.GetCriticalLevelCbo().Result;
            ViewData["lstCri"] = lstCri;
            var criDefault = lstCri.FirstOrDefault(x => x.IsDefault == true);
            if (criDefault != null)
                objProblem.CriticalLevelID = criDefault.ID;
            else if (lstCri.Count > 0)
                objProblem.CriticalLevelID = lstCri[0].ID;

            objProblem.ResidentAgencyGroupID = 0;
            objProblem.ResidentAgencyID = 0;
            objProblem.RequestDepID = 0;

            objProblem.OnDuty = true;
            objProblem.OnDutyText = "true";

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

            // Gán giá trị mặc định
            objProblem.Code = string.Format("SC{0}", long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
            //return View();
            objProblem.Receiver = User.EmployeeID;
            objProblem.ReceiverName = User.Name;

            // gán trạng thái mặc định
            List<ProblemStatusDTO> lstStatus = api.GetStatusCbo().Result;
            ViewData["lstStatus"] = lstStatus;
            var statusDefault = lstStatus.FirstOrDefault(x => x.IsDefault == true);
            if (statusDefault != null)
                objProblem.StatusID = statusDefault.ID;
            else if (lstStatus.Count > 0)
                objProblem.StatusID = lstStatus[0].ID;

            // gán kiểu mặc định
            List<ProblemTypeDTO> lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemEmergencyDTO> lstEmergency = api.GetEmergencyCbo().Result;
            ViewData["lstEmergency"] = lstEmergency;
            var emergencyDefault = lstEmergency.FirstOrDefault(x => x.IsDefault == true);
            if (emergencyDefault != null)
                objProblem.EmergencyTypeID = emergencyDefault.ID;
            else if (lstEmergency.Count > 0)
                objProblem.EmergencyTypeID = lstEmergency[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemCriticalLevelDTO> lstCri = api.GetCriticalLevelCbo().Result;
            ViewData["lstCri"] = lstCri;
            var criDefault = lstCri.FirstOrDefault(x => x.IsDefault == true);
            if (criDefault != null)
                objProblem.CriticalLevelID = criDefault.ID;
            else if (lstCri.Count > 0)
                objProblem.CriticalLevelID = lstCri[0].ID;

            // Lấy danh sách nhóm đơn vị thường trú
            List<ProblemResidentAgencyGroupDTO> lstResidentAgencyGroup = api.GetResidentAgencyGroupCbo().Result;
            if (lstResidentAgencyGroup == null)
                lstResidentAgencyGroup = new List<ProblemResidentAgencyGroupDTO>();
            lstResidentAgencyGroup.Insert(0, new ProblemResidentAgencyGroupDTO() { ID = 0, Name = "Chọn ..." });
            ViewData["lstResidentAgencyGroup"] = lstResidentAgencyGroup;
            
            objProblem = api.GetByID(ID);
            if(objProblem != null)
            {
                objProblem.OnDutyText = "false";
                if (objProblem.OnDuty == true)
                    objProblem.OnDutyText = "true";
            }

            // file 
            //var problemEventItem = problemEventSV.GetById(ID);
            //objProblem.AttachmentFiles = problemEventItem.AttachmentFiles;
            objProblem.AttachmentFiles = problemEventSV.getAttachFile(ID);

            if(objProblem.ResidentAgencyGroupID == null)
                objProblem.ResidentAgencyGroupID = 0;
            if(objProblem.ResidentAgencyID == null)
                objProblem.ResidentAgencyID = 0;
            if(objProblem.RequestDepID == null)
                objProblem.RequestDepID = 0;

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

            // Gán giá trị mặc định
            objProblem.Code = string.Format("SC{0}", long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
            //return View();
            objProblem.Receiver = User.EmployeeID;
            objProblem.ReceiverName = User.Name;

            // gán trạng thái mặc định
            List<ProblemStatusDTO> lstStatus = api.GetStatusCbo().Result;
            ViewData["lstStatus"] = lstStatus;
            var statusDefault = lstStatus.FirstOrDefault(x => x.IsDefault == true);
            if (statusDefault != null)
                objProblem.StatusID = statusDefault.ID;
            else if (lstStatus.Count > 0)
                objProblem.StatusID = lstStatus[0].ID;

            // gán kiểu mặc định
            List<ProblemTypeDTO> lstType = api.GetTypeCbo().Result;
            ViewData["lstType"] = lstType;
            var typeDefault = lstType.FirstOrDefault(x => x.IsDefault == true);
            if (typeDefault != null)
                objProblem.ProblemTypeID = typeDefault.ID;
            else if (lstType.Count > 0)
                objProblem.ProblemTypeID = lstType[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemEmergencyDTO> lstEmergency = api.GetEmergencyCbo().Result;
            ViewData["lstEmergency"] = lstEmergency;
            var emergencyDefault = lstEmergency.FirstOrDefault(x => x.IsDefault == true);
            if (emergencyDefault != null)
                objProblem.EmergencyTypeID = emergencyDefault.ID;
            else if (lstEmergency.Count > 0)
                objProblem.EmergencyTypeID = lstEmergency[0].ID;

            //Gán độ khẩn mặc định
            List<ProblemCriticalLevelDTO> lstCri = api.GetCriticalLevelCbo().Result;
            ViewData["lstCri"] = lstCri;
            var criDefault = lstCri.FirstOrDefault(x => x.IsDefault == true);
            if (criDefault != null)
                objProblem.CriticalLevelID = criDefault.ID;
            else if (lstCri.Count > 0)
                objProblem.CriticalLevelID = lstCri[0].ID;

            // Lấy danh sách nhóm đơn vị thường trú
            List<ProblemResidentAgencyGroupDTO> lstResidentAgencyGroup = api.GetResidentAgencyGroupCbo().Result;
            if (lstResidentAgencyGroup == null)
                lstResidentAgencyGroup = new List<ProblemResidentAgencyGroupDTO>();
            lstResidentAgencyGroup.Insert(0, new ProblemResidentAgencyGroupDTO() { ID = 0, Name = "Chọn ..." });
            ViewData["lstResidentAgencyGroup"] = lstResidentAgencyGroup;

            objProblem = api.GetByID(ID);
            if (objProblem != null)
            {
                objProblem.OnDutyText = "false";
                if (objProblem.OnDuty == true)
                    objProblem.OnDutyText = "true";
            }

            // file 
            //var problemEventItem = problemEventSV.GetById(ID);
            //objProblem.AttachmentFiles = problemEventItem.AttachmentFiles;
            objProblem.AttachmentFiles = problemEventSV.getAttachFile(ID);

            if (objProblem.ResidentAgencyGroupID == null)
                objProblem.ResidentAgencyGroupID = 0;
            if (objProblem.ResidentAgencyID == null)
                objProblem.ResidentAgencyID = 0;
            if (objProblem.RequestDepID == null)
                objProblem.RequestDepID = 0;

            ViewData["ReadOnly"] = true;

            return new Ext.Net.MVC.PartialViewResult
            {
                Model = objProblem,
                ViewData = ViewData,
                ViewName = "UpdateWindow"
            };
        }

        public ActionResult GetData(StoreRequestParameters parameters, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, string choise = null, int? type = null, int? group = null, int departmentID = 0)
        {
            bool isProblemOrEvent = true;
            if (choise != null && choise.Equals("App.All"))
            {
                fromDate = null;
                toDate = null;
            }
            else if (fromDate == null && toDate == null)
            {
                var date = DateTime.Now.Date;
                fromDate = new DateTime(date.Year, date.Month, 1);
                toDate = new DateTime(date.Year, 12, 31);
            }
            var resp = api.GetData(parameters.Page, parameters.Limit, false, fromDate, toDate, type, group, departmentID);
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
        public async Task<ActionResult> GetResidentAgencyGroupCbo()
        {
            var result = await api.GetResidentAgencyGroupCbo();
            if (result == null)
                result = new List<ProblemResidentAgencyGroupDTO>();
            result.Insert(0, new ProblemResidentAgencyGroupDTO() { ID = 0, Name = "Chọn ..." });
            return this.Store(result);
        }
        
        public async Task<ActionResult> GetResidentAgencyCbo(int groupID = 0)
        {
            var result = await api.GetResidentAgencyCbo(groupID);
            if (result == null)
                result = new List<ProblemResidentAgencyDTO>();
            result.Insert(0, new ProblemResidentAgencyDTO() { ID = 0, Name = "Chọn ..." });
            return this.Store(result);
        }
        public async Task<ActionResult> GetGroupCbo(int typeID = 0)
        {
            var result = await api.GetGroupCbo(typeID);
            return this.Store(result);
        }
        public async Task<ActionResult> GetRequestDepCbo()
        {
            var result = await api.GetRequestDepCbo();
            if (result == null)
                result = new List<ProblemEventRequestDepDTO>();
            result.Insert(0, new ProblemEventRequestDepDTO() { ID = 0, Name = "Chọn ..." });
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

        //insert & update sự cố
        public ActionResult Insert(ProblemEventDTO objNew, string data = "", bool createFromTemp = false)
        {
            // nếu tạo mới từ mẫu sự cố
            if (createFromTemp)
                objNew.ID = 0;

            bool success = false;
            int problemEventID = 0;
            if (objNew.ID > 0)
                problemEventID = objNew.ID.Value;

            try
            {
                if (objNew.Receiver <= 0)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Bạn phải chọn người tiếp nhận"
                    });
                    return this.Direct();
                }
                else if(string.IsNullOrWhiteSpace(data) && (objNew.YourselfFix == false || objNew.YourselfFix == null))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Bạn phải chọn phòng ban xử lý"
                    });
                    return this.Direct();
                }
                else
                {
                    data = data.Trim(',');
                    var attachments = JSON.Deserialize<List<int>>(data);

                    bool isUpdate = objNew.ID > 0;
                    
                    if(objNew.ResidentAgencyGroupID == 0)
                        objNew.ResidentAgencyGroupID = null;
                    if(objNew.ResidentAgencyID == 0)
                        objNew.ResidentAgencyID = null;
                    if(objNew.RequestDepID == 0)
                        objNew.RequestDepID = null;

                    objNew.OnDuty = false;
                    if (objNew.OnDutyText.Equals("true"))
                        objNew.OnDuty = true;
                    if (objNew.YourselfFix == null)
                        objNew.YourselfFix = false;

                    ResponseModel<object> result = api.InsertUpdate(objNew, User.ID, false);
                    if (result.MessageCode != null && result.MessageCode.Equals("0000"))
                    {
                        if(problemEventID == 0)
                            problemEventID = int.Parse(result.Data.ToString());

                        //update danh sách phòng ban xử lý
                        if(attachments != null && attachments.Count > 0)
                            UpdateProblemDepartment(problemEventID, attachments, isUpdate);
                        //update file
                        new FileSV().Upload<V3ProblemAttachmentFile>(objNew.AttachmentFiles, problemEventID);

                        success = true;
                        if (isUpdate)
                            Ultility.ShowNotification(message: RequestMessage.UpdateSuccess);
                        else
                            Ultility.ShowNotification(message: RequestMessage.CreateSuccess);
                    }    
                    else
                        Ultility.ShowNotification(message: RequestMessage.Error);
                }
            }
            catch(Exception ex)
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.FormPanel(success);
        }

        private bool UpdateProblemDepartment(int problemEventID, List<int> lstDepartmentID, bool isUpdate)
        {
            try
            {
                if (isUpdate)
                    api.InsertUpdateProblemDepartment(lstDepartmentID, problemEventID, User.ID);
                else
                {
                    for (int i = 0; i < lstDepartmentID.Count; i++)
                    {
                        api.InsertProblemDepartment(lstDepartmentID[i], problemEventID, User.ID);
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
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
            ViewData["ReadOnly"] = false;

            return new Ext.Net.MVC.PartialViewResult { Model = data, ViewData = ViewData };
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

        public ActionResult GetDepartmentListByProblem(int problemEventID)
        {
            var resp = api.GetDepartmentByProblemEvent(problemEventID).Result;
            return this.Store(resp);
        }

        private List<ProblemEventReport> getExportData(int problemEventID)
        {
            ProblemEventDTO objProblemEvent = new ProblemEventDTO();
            objProblemEvent = api.GetByID(problemEventID);

            List<ProblemEventReportDTO> lstDetail = apiRpt.GetData(problemEventID).Data;

            List<ProblemEventReport> lstRPT = new List<ProblemEventReport>();
            if(lstDetail != null && lstDetail.Count > 0)
            {
                for(int i=0; i<lstDetail.Count; i++)
                {
                    ProblemEventReport objReport = new ProblemEventReport();
                    objReport.getData(objProblemEvent);
                    objReport.ReporterName1 = lstDetail[i].ReporterName;
                    objReport.ReportDate1 = lstDetail[i].ReportDate.ToString("dd/MM/yyyy HH:mm");
                    objReport.From1 = lstDetail[i].From.ToString("dd/MM/yyyy HH:mm");
                    objReport.To1 = lstDetail[i].To.ToString("dd/MM/yyyy HH:mm");
                    objReport.Percent1 = lstDetail[i].Percent;
                    objReport.Content1 = lstDetail[i].Content;
                    lstRPT.Add(objReport);
                }
            }
            else
            {
                ProblemEventReport objReport = new ProblemEventReport();
                objReport.getData(objProblemEvent);
                lstRPT.Add(objReport);
            }

            return lstRPT;
        }

        public ActionResult ExportPDF(int problemEventID)
        {
            try
            {
                string fileName = string.Format("ThongTinSuCo{0}.pdf", problemEventID);
                var data = getExportData(problemEventID);

                ProblemEventDetailRPT rpt = new ProblemEventDetailRPT();
                rpt.Load();
                rpt.SetDataSource(data);
                Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(s, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExportWord(int problemEventID)
        {
            try
            {
                string fileName = string.Format("ThongTinSuCo{0}.doc", problemEventID);
                var data = getExportData(problemEventID);

                ProblemEventDetailRPT rpt = new ProblemEventDetailRPT();
                rpt.Load();
                rpt.SetDataSource(data);
                Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                return File(s, "application/msword", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Lấy người trực ca của phòng ban
        public ActionResult GetShiftDepartment(int departmentID)
        {
            string strResult = api.GetShiftDepartment(departmentID).Result;
            return this.Direct(strResult);
        }

        public ActionResult GetGrid(int id)
        {
            var result = api.GetUserFix(id).Result;
            if(result != null)
                return this.ComponentConfig("InnerGrid", result);
            else
                return this.ComponentConfig("InnerGrid", new List<ProblemUserFix>());
        }

    }
}