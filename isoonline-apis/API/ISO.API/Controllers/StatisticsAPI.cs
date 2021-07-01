using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("API liên quan đến báo cáo thống kê")]
    public class StatisticsAPI : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsAPI(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult CriticalTypeReport([FromBody] RequestModel<CriticalTypeReqModel> requestModel)
        {
            try
            {
                List<CriticalTypeReportDTO> lst = _statisticsService.CriticalTypeReport(requestModel.Data.From, requestModel.Data.To);
                var lstCritical = lst.GroupBy(x => x.CriticalLevelID);
                List<CriticalTypeResModel> criticalTypeResModels = new List<CriticalTypeResModel>();
                foreach (var item in lstCritical)
                {
                    CriticalTypeResModel criticalTypeResModel = new CriticalTypeResModel();
                    criticalTypeResModel.CriticalLevelID = item.FirstOrDefault().CriticalLevelID;
                    criticalTypeResModel.CriticalLevelName = item.FirstOrDefault().CriticalLevelName;

                    List<Dep> deps = new List<Dep>();
                    var lstDep = item.GroupBy(y => y.DepartmentID);
                    foreach (var sub in lstDep)
                    {
                        Dep dep = new Dep();
                        dep.DepartmentID = sub.FirstOrDefault().DepartmentID;
                        dep.DepName = sub.FirstOrDefault().DepName;
                        dep.EventCount = sub.FirstOrDefault().Count;
                        deps.Add(dep);
                    }
                    criticalTypeResModel.Departments = deps;
                    criticalTypeResModels.Add(criticalTypeResModel);
                }
                ResponseModel<List<CriticalTypeResModel>> response = new ResponseModel<List<CriticalTypeResModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = criticalTypeResModels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult CriticalTypeReportDetail([FromBody] RequestModel<CriticalTypeReportDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> lst = _statisticsService.CriticalTypeReport_Detail(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lst,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult CriticalTypeReportEventRequestDepDetail([FromBody] RequestModel<CriticalTypeReportEventRequestDepDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> lst = _statisticsService.CriticalTypeReportEventRequestDep_Detail(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lst,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult DepartmentReportResidentAgencyDetail([FromBody] RequestModel<DepartmentReportResidentAgencyDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> lst = _statisticsService.DepartmentReportResidentAgency_Detail(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lst,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }


        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemTypeReport([FromBody] RequestModel<ProblemTypeReqModel> requestModel)
        {
            try
            {
                List<ProblemTypeReportDTO> lst = _statisticsService.ProblemTypeReport(requestModel.Data.From, requestModel.Data.To);
                var lstProblemType = lst.GroupBy(x => x.ProblemTypeID);
                List<ProblemTypeResModel> problemTypeResModels = new List<ProblemTypeResModel>();
                foreach (var item in lstProblemType)
                {
                    ProblemTypeResModel problemTypeResModel = new ProblemTypeResModel();
                    problemTypeResModel.ProblemTypeID = item.FirstOrDefault().ProblemTypeID;
                    problemTypeResModel.ProblemTypeName = item.FirstOrDefault().ProblemTypeName;

                    List<Dep> deps = new List<Dep>();
                    var lstDep = item.GroupBy(y => y.DepartmentID);
                    foreach (var sub in lstDep)
                    {
                        Dep dep = new Dep();
                        dep.DepartmentID = sub.FirstOrDefault().DepartmentID;
                        dep.DepName = sub.FirstOrDefault().DepName;
                        dep.EventCount = sub.FirstOrDefault().Count;
                        deps.Add(dep);
                    }
                    problemTypeResModel.Departments = deps;
                    problemTypeResModels.Add(problemTypeResModel);
                }
                ResponseModel<List<ProblemTypeResModel>> response = new ResponseModel<List<ProblemTypeResModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = problemTypeResModels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemTypeReportDetail([FromBody] RequestModel<ProblemTypeReportDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> lst = _statisticsService.ProblemTypeReport_Detail(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lst,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }


        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemTypeReportEventRequestDepDetail([FromBody] RequestModel<ProblemTypeReportEventRequestDepDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> lst = _statisticsService.ProblemTypeReportEventRequestDep_Detail(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lst,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemTypeReportEventResidentAgencyDetail([FromBody] RequestModel<ProblemTypeReportResidentAgencyDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> lst = _statisticsService.ProblemTypeReportResidentAgency_Detail(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lst,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }
        //[HttpPost]
        //[Route("[action]")]
        //public IActionResult ProblemEventUser_ReportByUser([FromBody] RequestModel<ProblemEventUserReportReqModel> requestModel)
        //{
        //    try
        //    {
        //        List<ProblemEventUserReportDTO> lst = _statisticsService.ProblemEventUser_ReportByUser(requestModel.Data.From, requestModel.Data.To);
        //        var lstUser = lst.GroupBy(x => x.UserID);
        //        List<ProblemEventUserReportResModel> problemEventUserReportResModels = new List<ProblemEventUserReportResModel>();
        //        foreach (var item in lstUser)
        //        {
        //            ProblemEventUserReportResModel problemEventUserReportResModel = new ProblemEventUserReportResModel();
        //            problemEventUserReportResModel.UserID = item.FirstOrDefault().UserID;
        //            problemEventUserReportResModel.UserName = item.FirstOrDefault().UserName;
        //            problemEventUserReportResModel.DepartmentID = item.FirstOrDefault().DepartmentID;
        //            problemEventUserReportResModel.DepName = item.FirstOrDefault().DepName;

        //            List<ProblemType> types = new List<ProblemType>();
        //            var lstType = item.GroupBy(y => y.ProblemTypeID);
        //            foreach (var sub in lstType)
        //            {
        //                ProblemType type = new ProblemType();
        //                type.ProblemTypeID = sub.FirstOrDefault().ProblemTypeID;
        //                type.ProblemTypeName = sub.FirstOrDefault().ProblemTypeName;
        //                type.EventCount = sub.FirstOrDefault().Count;
        //                types.Add(type);
        //            }
        //            problemEventUserReportResModel.ProblemTypes = types;
        //            problemEventUserReportResModels.Add(problemEventUserReportResModel);
        //        }
        //        ResponseModel<List<ProblemEventUserReportResModel>> response = new ResponseModel<List<ProblemEventUserReportResModel>>()
        //        {
        //            MessageCode = "0000",
        //            Message = "Success!",
        //            Data = problemEventUserReportResModels
        //        };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        ResponseModel<string> response = new ResponseModel<string>()
        //        {
        //            MessageCode = "0001",
        //            Message = ex.Message.ToString(),
        //            Data = string.Empty
        //        };
        //        return Ok(response);
        //    }
        //}
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemEvent_ReportByDepartment([FromBody] RequestModel<ProblemEventReport_ByDepartmentReqModel> requestModel)
        {
            try
            {
                var data = _statisticsService.ProblemEventReport_ByDepartment(requestModel.Data.From, requestModel.Data.To, requestModel.Data.DepartmentID);

                ResponseModel<ProblemEventReport_ByDepartment> response = new ResponseModel<ProblemEventReport_ByDepartment>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }


        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemEvent_ReportByDepartmentDetail([FromBody] RequestModel<ProblemEventReport_ByDepDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> data = _statisticsService.ProblemEventReport_ByDepDetail(out int totalRows, requestModel.Data);

                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemEvent_ReportByUser([FromBody] RequestModel<ProblemEventReport_GetByUserReqModel> requestModel)
        {
            try
            {
                var data = _statisticsService.ProblemEventReport_GetByUser(requestModel.Data);

                ResponseModel<List<ProblemEventReport_GetByUserDTO>> response = new ResponseModel<List<ProblemEventReport_GetByUserDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemEvent_ReportByUserDetail([FromBody] RequestModel<ProblemEventReport_GetByUserDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> data = _statisticsService.ProblemEventReport_GetByUserDetail(out int totalRows, requestModel.Data);

                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult CriticalTypeReportEventRequest([FromBody] RequestModel<CriticalTypeReqModel> requestModel)
        {
            try
            {
                List<ProblemEventReport_ByEventRequestDepCriticalDTO> lst = _statisticsService.CriticalTypeReportEventRequest(requestModel.Data.From, requestModel.Data.To);
                var lstCritical = lst.GroupBy(x => x.CriticalLevelID);
                List<ProblemCriticalEventRequestDepResModel> criticalTypeResModels = new List<ProblemCriticalEventRequestDepResModel>();
                foreach (var item in lstCritical)
                {
                    ProblemCriticalEventRequestDepResModel criticalTypeResModel = new ProblemCriticalEventRequestDepResModel();
                    criticalTypeResModel.ProblemCriticalLevelID = item.FirstOrDefault().CriticalLevelID;
                    criticalTypeResModel.ProblemCriticalLevelName = item.FirstOrDefault().CriticalLevelName;

                    List<EventReqDep> eventReqDeps = new List<EventReqDep>();
                    var lstDep = item.GroupBy(y => y.RequestDepId);
                    foreach (var sub in lstDep)
                    {
                        EventReqDep eventReqDep = new EventReqDep();
                        eventReqDep.EventRequestDepID = sub.FirstOrDefault().RequestDepId;
                        eventReqDep.EventRequestDepName = sub.FirstOrDefault().Name;
                        eventReqDep.EventCount = sub.FirstOrDefault().Count;
                        eventReqDeps.Add(eventReqDep);
                    }
                    criticalTypeResModel.EventRequestDep = eventReqDeps;
                    criticalTypeResModels.Add(criticalTypeResModel);
                }
                ResponseModel<List<ProblemCriticalEventRequestDepResModel>> response = new ResponseModel<List<ProblemCriticalEventRequestDepResModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = criticalTypeResModels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemTypeReportEventRequest([FromBody] RequestModel<ProblemTypeReqModel> requestModel)
        {
            try
            {
                List<ProblemEventReport_ByEventRequestDepTypeDTO> lst = _statisticsService.ProblemTypeReportEventRequest(requestModel.Data.From, requestModel.Data.To);
                var lstProblemType = lst.GroupBy(x => x.ProblemTypeID);
                List<ProblemTypeEventRequestDepResModel> problemTypeResModels = new List<ProblemTypeEventRequestDepResModel>();
                foreach (var item in lstProblemType)
                {
                    ProblemTypeEventRequestDepResModel problemTypeResModel = new ProblemTypeEventRequestDepResModel();
                    problemTypeResModel.ProblemTypeID = item.FirstOrDefault().ProblemTypeID;
                    problemTypeResModel.ProblemTypeName = item.FirstOrDefault().ProblemTypeName;

                    List<EventReqDep> eventReqDeps = new List<EventReqDep>();
                    var lstDep = item.GroupBy(y => y.RequestDepId);
                    foreach (var sub in lstDep)
                    {
                        EventReqDep eventReqDep = new EventReqDep();
                        eventReqDep.EventRequestDepID = sub.FirstOrDefault().RequestDepId;
                        eventReqDep.EventRequestDepName = sub.FirstOrDefault().Name;
                        eventReqDep.EventCount = sub.FirstOrDefault().Count;
                        eventReqDeps.Add(eventReqDep);
                    }
                    problemTypeResModel.EventRequestDep = eventReqDeps;
                    problemTypeResModels.Add(problemTypeResModel);
                }
                ResponseModel<List<ProblemTypeEventRequestDepResModel>> response = new ResponseModel<List<ProblemTypeEventRequestDepResModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = problemTypeResModels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult DepartmentReportResidentAgency([FromBody] RequestModel<CriticalTypeReqModel> requestModel)
        {
            try
            {
                List<ProblemEventReport_ByResidentAgencyDepartmentDTO> lst = _statisticsService.DepartmentReportResidentAgency(requestModel.Data.From, requestModel.Data.To);
                var lstDepartment = lst.GroupBy(x => x.DepartmentID);
                List<DepartmentEventResidentAgencyResModel> departmentResModels = new List<DepartmentEventResidentAgencyResModel>();
                foreach (var item in lstDepartment)
                {
                    DepartmentEventResidentAgencyResModel departmentResModel = new DepartmentEventResidentAgencyResModel();
                    departmentResModel.DepartmentID = item.FirstOrDefault().DepartmentID;
                    departmentResModel.DepartmentName = item.FirstOrDefault().DepartmentName;

                    List<ResidentAgency> residentAgencies = new List<ResidentAgency>();
                    var lstDep = item.GroupBy(y => y.DepartmentID);
                    foreach (var sub in lstDep)
                    {
                        ResidentAgency residentAgency = new ResidentAgency();
                        residentAgency.ResidentAgencyID = sub.FirstOrDefault().ResidentAgencyID;
                        residentAgency.ResidentAgencyName = sub.FirstOrDefault().ResidentAgencyName;
                        residentAgency.EventCount = sub.FirstOrDefault().Count;
                        residentAgencies.Add(residentAgency);
                    }
                    departmentResModel.ResidentAgencies = residentAgencies;
                    departmentResModels.Add(departmentResModel);
                }
                ResponseModel<List<DepartmentEventResidentAgencyResModel>> response = new ResponseModel<List<DepartmentEventResidentAgencyResModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = departmentResModels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemTypeReportResidentAgency([FromBody] RequestModel<ProblemTypeReqModel> requestModel)
        {
            try
            {
                List<ProblemEventReport_ByResidentAgencyTypeDTO> lst = _statisticsService.ProblemTypeReportResidentAgency(requestModel.Data.From, requestModel.Data.To);
                var lstProblemType = lst.GroupBy(x => x.ProblemTypeID);
                List<ProblemTypeEventResidentAgencyResModel> problemTypeResModels = new List<ProblemTypeEventResidentAgencyResModel>();
                foreach (var item in lstProblemType)
                {
                    ProblemTypeEventResidentAgencyResModel problemTypeResModel = new ProblemTypeEventResidentAgencyResModel();
                    problemTypeResModel.ProblemTypeID = item.FirstOrDefault().ProblemTypeID;
                    problemTypeResModel.ProblemTypeName = item.FirstOrDefault().ProblemTypeName;

                    List<ResidentAgency> residentAgencies = new List<ResidentAgency>();
                    var lstDep = item.GroupBy(y => y.ResidentAgencyID);
                    foreach (var sub in lstDep)
                    {
                        ResidentAgency residentAgency = new ResidentAgency();
                        residentAgency.ResidentAgencyID = sub.FirstOrDefault().ResidentAgencyID;
                        residentAgency.ResidentAgencyName = sub.FirstOrDefault().ResidentAgencyName;
                        residentAgency.EventCount = sub.FirstOrDefault().Count;
                        residentAgencies.Add(residentAgency);
                    }
                    problemTypeResModel.ResidentAgencies = residentAgencies;
                    problemTypeResModels.Add(problemTypeResModel);
                }
                ResponseModel<List<ProblemTypeEventResidentAgencyResModel>> response = new ResponseModel<List<ProblemTypeEventResidentAgencyResModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = problemTypeResModels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemOnDutyReport([FromBody] RequestModel<ProblemOnDutyReqModel> requestModel)
        {
            try
            {
                List<ProblemOnDutyReportDTO> lst = _statisticsService.ProblemOnDutyReport(requestModel.Data.From, requestModel.Data.To);

                var lstProblemType = lst.GroupBy(x => x.OnDuty);

                List<ProblemOnDutyResModel> problemTypeResModels = new List<ProblemOnDutyResModel>();
                foreach (var item in lstProblemType)
                {
                    ProblemOnDutyResModel problemTypeResModel = new ProblemOnDutyResModel();
                    problemTypeResModel.OnDuty = item.FirstOrDefault().OnDuty;

                    List<Dep> deps = new List<Dep>();
                    var lstDep = item.GroupBy(y => y.DepartmentID);
                    foreach (var sub in lstDep)
                    {
                        Dep dep = new Dep();
                        dep.DepartmentID = sub.FirstOrDefault().DepartmentID;
                        dep.DepName = sub.FirstOrDefault().DepName;
                        dep.EventCount = sub.FirstOrDefault().Count;
                        deps.Add(dep);
                    }
                    problemTypeResModel.Departments = deps;
                    problemTypeResModels.Add(problemTypeResModel);
                }
                ResponseModel<List<ProblemOnDutyResModel>> response = new ResponseModel<List<ProblemOnDutyResModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = problemTypeResModels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult ProblemOnDutyReportDetail([FromBody] RequestModel<ProblemOnDutyReportDetailReqModel> requestModel)
        {
            try
            {
                List<ProblemEventDTO> lst = _statisticsService.ProblemOnDutyReport_Detail(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventDTO>> response = new ResponseModel<List<ProblemEventDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lst,
                    TotalRows = totalRows
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }

    }
}
