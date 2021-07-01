using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ISO.API.Middleware;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("API liên quan đến sự cố, sự kiện")]
    public class ProblemEventAPI : ControllerBase
    {
        private readonly IProblemEventService _problemEventService;
        private readonly ILogger<ProblemEventAPI> _logger;
        public ProblemEventAPI(IProblemEventService problemEventService, ILogger<ProblemEventAPI> logger)
        {
            _problemEventService = problemEventService;
            _logger = logger;
        }
        /// <summary>Danh sách Sự cố, sự kiện có phân trang</summary>
        /// <remarks></remarks>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetPaging([FromBody] RequestModel<ProblemEventReqModel> requestModel)
        {
            try
            {
                //int rs = _problemEventService.InsertProblemEventUser();
                List<ProblemEventDTO> data = _problemEventService.GetPaging(out int totalRows, requestModel.Data);
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
        /// <summary>Thêm mới, cập nhật Sự cố, sự kiện</summary>
        /// <remarks></remarks>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult InsertUpdate([FromBody] RequestModel<ProblemEventDTO> requestModel)
        {
            try
            {
                int rs = _problemEventService.InsertUpdate(requestModel.Data);
                ResponseModel<int> response = new ResponseModel<int>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = rs
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }
        /// <summary>Lấy Sự kiện, sự cố theo ID</summary>
        /// <remarks></remarks>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByID([FromBody] RequestModel<ProblemEventReqModel> requestModel)
        {
            try
            {
                if (requestModel.Data.ID.HasValue)
                {
                    ProblemEventDTO data = _problemEventService.GetByID(requestModel.Data.ID.Value);
                    ResponseModel<ProblemEventDTO> response = new ResponseModel<ProblemEventDTO>()
                    {
                        MessageCode = "0000",
                        Message = "Success!",
                        Data = data
                    };
                    return Ok(response);
                }
                else
                {
                    ResponseModel<string> response = new ResponseModel<string>()
                    {
                        MessageCode = "0001",
                        Message = "Input is missing!",
                        Data = string.Empty
                    };
                    return Ok(response);
                }
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
        /// <summary>Xoá Sự kiện, sự cố theo ID</summary>
        /// <remarks></remarks>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Delete([FromBody] RequestModel<ProblemEventReqModel> requestModel)
        {
            try
            {
                if (requestModel.Data.ID.HasValue)
                {
                    int data = _problemEventService.Delete(requestModel.Data.ID.Value);
                    ResponseModel<int> response = new ResponseModel<int>()
                    {
                        MessageCode = "0000",
                        Message = "Success!",
                        Data = data
                    };
                    return Ok(response);
                }
                else
                {
                    ResponseModel<string> response = new ResponseModel<string>()
                    {
                        MessageCode = "0001",
                        Message = "Input is missing!",
                        Data = string.Empty
                    };
                    return Ok(response);
                }
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
