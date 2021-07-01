using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("API liên quan đến trạng thái sự cố")]
    public class ProblemStatusAPI : ControllerBase
    {
        private readonly IProblemStatusService _problemStatusService;
        public ProblemStatusAPI(IProblemStatusService problemStatusService)
        {
            _problemStatusService = problemStatusService;
        }
        /// <summary>Danh sách Trạng thái xử lý của sự cố có phân trang</summary>
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
        public IActionResult GetPaging([FromBody] RequestModel<ProblemStatusReqModel> requestModel)
        {
            try
            {
                List<ProblemStatusDTO> data = _problemStatusService.GetPaging(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemStatusDTO>> response = new ResponseModel<List<ProblemStatusDTO>>()
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
        /// <summary>Lấy Trạng thái xử lý của sự cố theo ID</summary>
        /// <remarks></remarks>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByID(int ID)
        {
            try
            {
                ProblemStatusDTO data = _problemStatusService.GetByID(ID);
                ResponseModel<ProblemStatusDTO> response = new ResponseModel<ProblemStatusDTO>()
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
        /// <summary>Thêm mới, cập nhật Trạng thái xử lý của sự cố</summary>
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
        public IActionResult InsertUpdate([FromBody] RequestModel<ProblemStatusDTO> requestModel)
        {
            try
            {
                int rs = _problemStatusService.InsertUpdate(requestModel.Data);
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
                ResponseModel<string> response = new ResponseModel<string>()
                {
                    MessageCode = "0001",
                    Message = ex.Message.ToString(),
                    Data = string.Empty
                };
                return Ok(response);
            }
        }
        /// <summary>Xoá Trạng thái xử lý của sự cố theo ID</summary>
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
        public IActionResult Delete([FromBody] RequestModel<ProblemStatusReqModel> requestModel)
        {
            try
            {
                int affectedRows = _problemStatusService.Delete(requestModel.Data.ID);
                if (affectedRows != 0)
                {
                    ResponseModel<int> response = new ResponseModel<int>()
                    {
                        MessageCode = "0000",
                        Message = "Success!",
                        Data = affectedRows
                    };
                    return Ok(response);
                }
                else
                {
                    ResponseModel<int> response = new ResponseModel<int>()
                    {
                        MessageCode = "0000",
                        Message = "Trạng thái đang được sửa dụng!",
                        Data = affectedRows
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

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Default([FromBody] RequestModel<ProblemStatusDTO> requestModel)
        {
            try
            {
                int affectedRows = _problemStatusService.Default(requestModel.Data.ID);
                ResponseModel<int> response = new ResponseModel<int>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = affectedRows
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
