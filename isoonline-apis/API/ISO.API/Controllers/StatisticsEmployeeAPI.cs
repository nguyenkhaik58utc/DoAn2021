using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using ISO.API.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("API liên quan đến báo cáo thống kê nhân sự")]
    public class StatisticsEmployeeAPI : ControllerBase
    {
        private readonly IStatisticEmpoyeeService _statisticsService;
        private readonly IDepartmentTitleService _departmentTitleService;
        public StatisticsEmployeeAPI(IStatisticEmpoyeeService statisticsService, IDepartmentTitleService departmentTitleService)
        {
            _statisticsService = statisticsService;
            _departmentTitleService = departmentTitleService;
        }

        /// <summary>
        /// Thống kê nhân sự theo phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetEmployeeTotal()
        {
            try
            {
                List<EmployeeTotalTemp> lstDataTemp = _statisticsService.GetEmployeeTotal();
                // Đếm nhân sự (một nhân sự có thể ở nhiều phòng ban)
                List<EmployeeTotal> lstData = new List<EmployeeTotal>();
                for (int i = 0; i < lstDataTemp.Count; i++)
                {
                    if(!lstData.Exists(x => x.DepartmentID == lstDataTemp[i].DepartmentID) )
                    {
                        var t = lstDataTemp[i];
                        var obj = new EmployeeTotal() { DepartmentID = t.DepartmentID, DepartmentName = t.DepartmentName, ParentID = t.ParentID, EmpTotal = 0, Leaf = true };
                        if (obj.ParentID == 0)
                            obj.EmpTotal = lstDataTemp.Where(x=>x.EmpID > 0).Select(x => x.EmpID).Distinct().Count();
                        else
                            obj.EmpTotal = lstDataTemp.Where(x => x.DepartmentID == obj.DepartmentID || x.ParentID == obj.DepartmentID).Select(x => x.EmpID).Distinct().Count();

                        var lstChild = lstDataTemp.FindAll(x => x.ParentID == obj.DepartmentID);
                        if (lstChild != null && lstChild.Count > 0)
                            obj.Leaf = false;

                        lstData.Add(obj);
                    }    
                }

                ResponseModel<List<EmployeeTotal>> response = new ResponseModel<List<EmployeeTotal>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lstData
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

        ///<summary>Chi tiết thống kê nhân sự theo phòng ban</summary>
        ///<remarks></remarks>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetListEmp(int depID, int pageSize, int pageNumber)
        {
            try
            {
                List<HumanEmployeeResponse> humanEmployeeList = _statisticsService.GetListEmp(out int totalRows, pageSize, pageNumber, depID);
                foreach (var item in humanEmployeeList)
                {
                    item.ListDepartmentTitle = _departmentTitleService.GetListByEmployeeID(item.ID);
                }
                ResponseModel<List<HumanEmployeeResponse>> response = new ResponseModel<List<HumanEmployeeResponse>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = humanEmployeeList,
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

        /// <summary>
        /// Thống kê nhân sự theo chức danh
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetEmployeeTitleTotal()
        {
            try
            {
                List<EmployeeTitleTotal> lstData = _statisticsService.GetEmployeeTitleTotal();
                ResponseModel<List<EmployeeTitleTotal>> response = new ResponseModel<List<EmployeeTitleTotal>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = lstData
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

        ///<summary>Chi tiết thống kê nhân sự theo chức danh</summary>
        ///<remarks></remarks>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetListEmpByTitle(int titleID, int depID, int pageSize, int pageNumber)
        {
            try
            {
                List<HumanEmployeeResponse> humanEmployeeList = _statisticsService.GetListEmpByTitle(out int totalRows, pageSize, pageNumber, titleID, depID);
                foreach (var item in humanEmployeeList)
                {
                    item.ListDepartmentTitle = _departmentTitleService.GetListByEmployeeID(item.ID);
                }
                ResponseModel<List<HumanEmployeeResponse>> response = new ResponseModel<List<HumanEmployeeResponse>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = humanEmployeeList,
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
