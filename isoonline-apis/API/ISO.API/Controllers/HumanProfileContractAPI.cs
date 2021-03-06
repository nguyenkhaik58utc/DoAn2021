using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileContract;
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
    [SwaggerTag("API quản lý hồ sơ hợp đồng lao động")]
    [RsaAuthorize]
    public class HumanProfileContractAPI : ControllerBase
    {
        private readonly IHumanProfileContractService _iHumanProfileContractService;

        public HumanProfileContractAPI(IHumanProfileContractService iHumanProfileContractService)
        {
            _iHumanProfileContractService = iHumanProfileContractService;
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
                var data = _iHumanProfileContractService.GetByID(ID);
                ResponseModel<HumanProfileContractDTO> response = new ResponseModel<HumanProfileContractDTO>()
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
                var data = _iHumanProfileContractService.GetDetailByID(ID);
                ResponseModel<HumanProfileContractResponse> response = new ResponseModel<HumanProfileContractResponse>()
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
                var data = _iHumanProfileContractService.GetByEmployeeID(out int totalRows, pageSize, pageNumber, employeeID);
                ResponseModel<List<HumanProfileContractResponse>> response = new ResponseModel<List<HumanProfileContractResponse>>()
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
        /// Lấy toàn bộ danh sách hồ sơ hợp đồng theo EmployeeID ( không phân trang )
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllByEmployeeID( int employeeID)
        {
            try
            {
                var data = _iHumanProfileContractService.GetAllByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileContractResponse>> response = new ResponseModel<List<HumanProfileContractResponse>>()
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
        public IActionResult Insert([FromBody] RequestModel<HumanProfileContractDTO> request)
        {
            try
            {
                var data = _iHumanProfileContractService.Insert(request.Data);
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
        public IActionResult Update([FromBody] RequestModel<HumanProfileContractDTO> request)
        {
            try
            {
                var data = _iHumanProfileContractService.Update(request.Data);
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
                var data = _iHumanProfileContractService.Delete(request.Data.ID);
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