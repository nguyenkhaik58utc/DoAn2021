using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.ShiftHistory;
using ISO.API.Service;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace ISO.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [SwaggerTag("API lịch sử ca trực")]
    [RsaAuthorize]
    public class ShiftHistoryAPI : ControllerBase
    {

        private readonly IShiftHistoryService _shiftHistoryService;
        private readonly IProblemEventService _problemEventService;

        public ShiftHistoryAPI(IShiftHistoryService shiftHistoryService)
        {
            _shiftHistoryService = shiftHistoryService;
        }

        /// <summary>
        /// Lấy chi tiết lịch sử ca trực hiện tai của người dùng
        /// ductv 10/12/2020: Lấy số lượng sự cố ở phòng ban của mình để thông báo
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByUserID(int UserID)
        {
            try
            {
                var data = _shiftHistoryService.GetByUserID(UserID);
                if(data != null && data.ID > 0)
                    data.ProblemEventTotal = _shiftHistoryService.CountByUserID(UserID);
                ResponseModel<ShiftHistoryDTO> response = new ResponseModel<ShiftHistoryDTO>()
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
        /// Lấy danh sách ca trực hiện tại theo phòng ban
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByDepartmentID(int DepartmentID)
        {
            try
            {
                var data = _shiftHistoryService.GetByDepartmentID(DepartmentID);
                ResponseModel<string> response = new ResponseModel<string>()
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
        /// Thêm mới ca trực
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert(RequestModel<ShiftHistoryReqModel> request)
        {
            try
            {
                int result = _shiftHistoryService.Insert(request.Data);
                ResponseModel<int> response = new ResponseModel<int>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = result
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
        /// Cập nhật ca trực
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(RequestModel<ShiftHistoryReqModel> request)
        {
            try
            {
                var result = _shiftHistoryService.Update(request.Data);
                ResponseModel<int> response = new ResponseModel<int>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = 1
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
        /// Bàn giao ca trực
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult HandOverShift(RequestModel<ShiftHistoryReqModel> request)
        {
            try
            {
                var result = _shiftHistoryService.HandOverShift(request.Data);
                ResponseModel<int> response = new ResponseModel<int>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = 1
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
