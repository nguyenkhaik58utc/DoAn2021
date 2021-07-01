using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.WorkOut;
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
    [Route("api/[controller]")]
    [ApiController]
    [RsaAuthorize]
    [SwaggerTag("API liên quan đến đăng ký làm việc bên ngoài")]
    public class WorkOutAPI : ControllerBase
    {
        private readonly IWorkOutService _workOutService;

        public WorkOutAPI(IWorkOutService workOutService)
        {
            _workOutService = workOutService;
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult Insert([FromBody] RequestModel<InsertWorkOutRequest> request)
        {
            try
            {
                int result = _workOutService.Insert(request.Data);
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
        /// Cập nhật
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult Update([FromBody] RequestModel<UpdateWorkOutRequest> request)
        {
            try
            {
                int result = _workOutService.Update(request.Data);
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
        /// Xóa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[action]")]
        public IActionResult Delete(int id)
        {
            try
            {
                int result = _workOutService.Delete(id);
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
        /// Thêm mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult CheckExist([FromBody] RequestModel<CheckExistWorkOutRequest> request)
        {
            try
            {
                int result = _workOutService.CheckExist(request.Data);
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
        /// Lấy danh sách theo tháng và trạng thái
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetByMonth([FromQuery] GetWorkOutByMonthRequest request)
        {
            try
            {
                List<WorkOutViewModel> result = _workOutService.GetByMonth(out int totalRows, request);
                ResponseModel<List<WorkOutViewModel>> response = new ResponseModel<List<WorkOutViewModel>>()
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
        /// Lấy theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetById(int id)
        {
            try
            {
                WorkOutDTO result = _workOutService.GetById(id);
                ResponseModel<WorkOutDTO> response = new ResponseModel<WorkOutDTO>()
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