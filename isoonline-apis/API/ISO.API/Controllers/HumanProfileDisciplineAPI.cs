using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileDiscipline;
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
    [SwaggerTag("API quản lý hồ sơ kỷ luật")]
    [RsaAuthorize]
    public class HumanProfileDisciplineAPI : ControllerBase
    {
        private readonly IHumanProfileDisciplineService _iHumanProfileDisciplineService;

        public HumanProfileDisciplineAPI(IHumanProfileDisciplineService iHumanProfileDisciplineService)
        {
            _iHumanProfileDisciplineService = iHumanProfileDisciplineService;
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
                var data = _iHumanProfileDisciplineService.GetByID(ID);
                ResponseModel<HumanProfileDisciplineDTO> response = new ResponseModel<HumanProfileDisciplineDTO>()
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
                var data = _iHumanProfileDisciplineService.GetDetailByID(ID);
                ResponseModel<HumanProfileDisciplineResponse> response = new ResponseModel<HumanProfileDisciplineResponse>()
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
        public IActionResult GetByEmployeeID(int pageSize, int pageNumber, int employeeID)
        {
            try
            {
                var data = _iHumanProfileDisciplineService.GetByEmployeeID(out int totalRows, pageSize, pageNumber, employeeID);
                ResponseModel<List<HumanProfileDisciplineResponse>> response = new ResponseModel<List<HumanProfileDisciplineResponse>>()
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
        /// Lấy toàn bộ danh sách hồ sơ kỷ luật theo EmployeeID ( không phân trang )
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileDisciplineService.GetAllByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileDisciplineResponse>> response = new ResponseModel<List<HumanProfileDisciplineResponse>>()
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
        public IActionResult Insert([FromBody] RequestModel<HumanProfileDisciplineDTO> request)
        {
            try
            {
                var data = _iHumanProfileDisciplineService.Insert(request.Data);
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
        public IActionResult Update([FromBody] RequestModel<HumanProfileDisciplineDTO> request)
        {
            try
            {
                var data = _iHumanProfileDisciplineService.Update(request.Data);
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
                var data = _iHumanProfileDisciplineService.Delete(request.Data.ID);
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