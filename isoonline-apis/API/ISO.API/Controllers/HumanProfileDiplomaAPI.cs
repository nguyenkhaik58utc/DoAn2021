using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileDiploma;
using ISO.API.Models.ProfileHumanPermission;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [SwaggerTag("API quản lý hồ sơ văn bằng")]
    [RsaAuthorize]
    public class HumanProfileDiplomaAPI : ControllerBase
    {
        private readonly IHumanProfileDiplomaService _iHumanProfileDiplomaService;

        public HumanProfileDiplomaAPI(IHumanProfileDiplomaService iHumanProfileDiplomaService)
        {
            _iHumanProfileDiplomaService = iHumanProfileDiplomaService;
        }

        /// <summary>
        /// Lấy danh sách hồ sơ văn bằng theo list ID nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListDiplomaByEmpID(string lstEmployeeID)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.getListDiplomaByEmpID(lstEmployeeID);
                ResponseModel<List<HumanProfileDiplomaExcel>> response = new ResponseModel<List<HumanProfileDiplomaExcel>>()
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

        /// <summary>
        /// Lấy hồ sơ theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByID(int ID)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.GetByID(ID);
                ResponseModel<HumanProfileDiplomaDTO> response = new ResponseModel<HumanProfileDiplomaDTO>()
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

        /// <summary>
        /// Lấy chi tiết hồ sơ theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDetailByID(int ID)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.GetDetailByID(ID);
                ResponseModel<HumanProfileDiplomaResponse> response = new ResponseModel<HumanProfileDiplomaResponse>()
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

        /// <summary>
        /// Lấy toàn bộ danh sách hồ sơ theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.GetByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileDiplomaResponse>> response = new ResponseModel<List<HumanProfileDiplomaResponse>>()
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

        /// <summary>
        /// Lấy toàn bộ danh sách hồ sơ theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPagingByEmployeeID(int pageSize, int pageNumber, int employeeID)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.GetPagingByEmployeeID(out int totalRows, pageSize, pageNumber, employeeID);
                ResponseModel<List<HumanProfileDiplomaResponse>> response = new ResponseModel<List<HumanProfileDiplomaResponse>>()
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

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert([FromBody] RequestModel<HumanProfileDiplomaDTO> request)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.Insert(request.Data);
                ResponseModel<int> response = new ResponseModel<int>()
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

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Update([FromBody] RequestModel<HumanProfileDiplomaDTO> request)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.Update(request.Data);
                ResponseModel<int> response = new ResponseModel<int>()
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

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete([FromBody] RequestModel<DeleteRequest> request)
        {
            try
            {
                var data = _iHumanProfileDiplomaService.Delete(request.Data.ID);
                ResponseModel<int> response = new ResponseModel<int>()
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