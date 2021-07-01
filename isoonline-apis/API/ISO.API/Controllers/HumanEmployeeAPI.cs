using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using ISO.API.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("API liên quan đến Employee")]
    public class HumanEmployeeAPI : ControllerBase
    {
        private readonly IHumanEmployeeService _humanEmployeeService;
        private readonly IDepartmentTitleService _departmentTitleService;

        public HumanEmployeeAPI(IHumanEmployeeService humanEmployeeService, IDepartmentTitleService departmentTitleService)
        {
            _humanEmployeeService = humanEmployeeService;
            _departmentTitleService = departmentTitleService;
        }

        ///<summary>Danh sách Chức danh nhân viên theo phòng ban</summary>
        ///<remarks></remarks>
        ///<param name="requestModel"></param>
        ///<returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByDepartment(int departmentID)
        {
            try
            {
                List<HumanEmployeeDTO> data = _humanEmployeeService.GetByDepartment(departmentID);
                ResponseModel<List<HumanEmployeeDTO>> response = new ResponseModel<List<HumanEmployeeDTO>>()
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

        ///<summary>Danh sách nhân viên theo phòng ban</summary>
        ///<remarks></remarks>
        ///<param name="departmentID"></param>
        ///<returns></returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetListByDepartmentID([FromBody] RequestModel<HumanEmployeeSearchRequest> req)
        {
            try
            {
                List<HumanEmployeeResponse> humanEmployeeList = _humanEmployeeService.GetListByDepartmentID(out int totalRows, req.Data);
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

        ///<summary>Danh sách toàn bộ nhân viên</summary>
        ///<remarks></remarks>
        ///<param name="departmentID"></param>
        ///<returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetListAll(int pageSize, int pageNumber)
        {
            try
            {
                List<HumanEmployeeResponse> humanEmployeeList = _humanEmployeeService.GetListALL(out int totalRows, pageSize, pageNumber);
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

        ///<summary>Danh sách toàn bộ nhân viên theo listemployeeId</summary>
        ///<remarks></remarks>
        ///<param name="departmentID"></param>
        ///<returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByListEmployeeId(string lstID)
        {
            try
            {
                List<HumanEmployeeResponse> humanEmployeeList = _humanEmployeeService.GetByListEmployeeId(lstID);
                ResponseModel<List<HumanEmployeeResponse>> response = new ResponseModel<List<HumanEmployeeResponse>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = humanEmployeeList
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

        ///<summary>Danh sách toàn bộ nhân viên theo document
        ///</summary>
        ///<remarks></remarks>
        ///<param name="departmentID"></param>
        ///<returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByDocument(int documentId)
        {
            try
            {
                List<HumanEmployeeResponse> humanEmployeeList = _humanEmployeeService.GetByDocument(documentId);
                ResponseModel<List<HumanEmployeeResponse>> response = new ResponseModel<List<HumanEmployeeResponse>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = humanEmployeeList
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

        ///<summary>Nhân viên theo id</summary>
        ///<remarks></remarks>
        ///<param name="id"></param>
        ///<returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetById(int id)
        {
            try
            {
                HumanEmployeeResponse data = _humanEmployeeService.GetById(id);
                ResponseModel<HumanEmployeeResponse> response = new ResponseModel<HumanEmployeeResponse>()
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
    }
}