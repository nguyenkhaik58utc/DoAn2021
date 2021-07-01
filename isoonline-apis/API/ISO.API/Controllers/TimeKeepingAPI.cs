using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.TimeKeeping;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace ISO.API.Controllers
{
    /// <summary>
    /// Chấm công
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [RsaAuthorize]
    [SwaggerTag("API liên quan đến dữ liệu chấm công")]
    public class TimeKeepingAPI : ControllerBase
    {
        private readonly ITimeKeepingService _timeKeepingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeKeepingService"></param>
        public TimeKeepingAPI(ITimeKeepingService timeKeepingService)
        {
            _timeKeepingService = timeKeepingService;
        }

        /// <summary>
        /// Lấy danh sách theo tháng và nhân viên
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetByMonth([FromQuery] GetTimeKeepingByMonthRequest request)
        {
            try
            {
                List<TimeKeepingOfEmployeeViewModel> result = _timeKeepingService.GetByMonth(out int totalRows, request);
                ResponseModel<List<TimeKeepingOfEmployeeViewModel>> response = new ResponseModel<List<TimeKeepingOfEmployeeViewModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = result,
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
        /// Lấy danh sách giải trình theo tháng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetExplanationByMonth([FromQuery] GetTimeKeepingExplanationByMonthRequest request)
        {
            try
            {
                List<TimeKeepingExplanationViewModel> result = _timeKeepingService.GetExplanationByMonth(out int totalRows, request);
                ResponseModel<List<TimeKeepingExplanationViewModel>> response = new ResponseModel<List<TimeKeepingExplanationViewModel>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = result,
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteExPlanation(int id)
        {
            try
            {
                int result = _timeKeepingService.DeleteExplanation(id);
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
    }
}