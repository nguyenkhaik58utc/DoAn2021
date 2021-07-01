using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileTraining;
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
    [SwaggerTag("API quản lý hồ sơ đào tạo")]
    [RsaAuthorize]
    public class HumanProfileTrainingAPI : ControllerBase
    {
        private readonly IHumanProfileTrainingService _iHumanProfileTrainingService;

        public HumanProfileTrainingAPI(IHumanProfileTrainingService HumanProfileTrainingService)
        {
            _iHumanProfileTrainingService = HumanProfileTrainingService;
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
                var data = _iHumanProfileTrainingService.GetByID(ID);
                ResponseModel<HumanProfileTrainingDTO> response = new ResponseModel<HumanProfileTrainingDTO>()
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
        /// Lấy danh sách hồ sơ đào tạo theo ID nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListTrainingByEmpID(string lstEmployeeID)
        {
            try
            {
                var data = _iHumanProfileTrainingService.getListTrainingByEmpID(lstEmployeeID);
                ResponseModel<List<HumanProfileEducationTrainingExcel>> response = new ResponseModel<List<HumanProfileEducationTrainingExcel>>()
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
                var data = _iHumanProfileTrainingService.GetDetailByID(ID);
                ResponseModel<HumanProfileTrainingResponse> response = new ResponseModel<HumanProfileTrainingResponse>()
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
                var data = _iHumanProfileTrainingService.GetPagingByEmployeeID(out int totalRows, pageSize, pageNumber, employeeID);
                ResponseModel<List<HumanProfileTrainingResponse>> response = new ResponseModel<List<HumanProfileTrainingResponse>>()
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
        /// Lấy toàn bộ danh sách hồ sơ theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileTrainingService.GetByEmployeeID( employeeID);
                ResponseModel<List<HumanProfileTrainingResponse>> response = new ResponseModel<List<HumanProfileTrainingResponse>>()
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
        /// Thêm mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert([FromBody] RequestModel<HumanProfileTrainingDTO> request)
        {
            try
            {
                var data = _iHumanProfileTrainingService.Insert(request.Data);
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
        public IActionResult Update([FromBody] RequestModel<HumanProfileTrainingDTO> request)
        {
            try
            {
                var data = _iHumanProfileTrainingService.Update(request.Data);
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
                var data = _iHumanProfileTrainingService.Delete(request.Data.ID);
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