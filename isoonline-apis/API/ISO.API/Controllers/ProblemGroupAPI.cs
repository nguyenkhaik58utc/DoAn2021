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
    [SwaggerTag("API liên quan đến nhóm sự cố")]
    public class ProblemGroupAPI : ControllerBase
    {
        private readonly IProblemGroupService _problemGroupService;
        public ProblemGroupAPI(IProblemGroupService problemGroupService)
        {
            _problemGroupService = problemGroupService;
        }
        /// <summary>Danh sách nhóm xử lý của sự cố có phân trang</summary>
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
        public IActionResult GetPaging([FromBody] RequestModel<ProblemGroupReqModel> requestModel)
        {
            try
            {
                List<ProblemGroupDTO> data = _problemGroupService.GetPaging(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemGroupDTO>> response = new ResponseModel<List<ProblemGroupDTO>>()
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
        /// <summary>Lấy nhóm xử lý của sự cố theo ID</summary>
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
                List<ProblemGroupDTO> data = _problemGroupService.GetByID(ID);
                ProblemGroupDTO result = new ProblemGroupDTO();
                if(data.Count != 0)
                {
                    result = data[0];
                    List<ProblemTypeDTO> problemType = new List<ProblemTypeDTO>();
                    for (int i = 0; i < data.Count; i++)
                    {
                        ProblemTypeDTO typeDTO = new ProblemTypeDTO();
                        typeDTO.ID = data[i].ProblemTypeID;
                        typeDTO.ProblemTypeName = data[i].ProblemGroupName;
                        problemType.Add(typeDTO);
                    }
                    result.lstType = problemType;
                }

                ResponseModel<ProblemGroupDTO> response = new ResponseModel<ProblemGroupDTO>()
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
        /// <summary>Thêm mới, cập nhật nhóm xử lý của sự cố</summary>
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
        public IActionResult InsertUpdate([FromBody] RequestModel<ProblemGroupDTO> requestModel)
        {
            try
            {
                int rs = _problemGroupService.InsertUpdate(requestModel.Data);
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
        /// <summary>Xoá nhóm xử lý của sự cố theo ID</summary>
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
        public IActionResult Delete([FromBody] RequestModel<ProblemGroupReqModel> requestModel)
        {
            try
            {
                int affectedRows = _problemGroupService.Delete(requestModel.Data.ID);
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
                        Message = "nhóm sự cố đang được sửa dụng!",
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
        /// <summary>Lấy nhóm xử lý của sự cố theo Loại</summary>
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
        public IActionResult GetByProblemType(int problemTypeID)
        {
            try
            {
                List<ProblemGroupDTO> data = _problemGroupService.GetByType(problemTypeID);
                ResponseModel<List<ProblemGroupDTO>> response = new ResponseModel<List<ProblemGroupDTO>>()
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
        /// <summary>Lấy nhóm xử lý của sự cố theo cha</summary>
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
        public IActionResult GetByParent(int parentID)
        {
            try
            {
                List<ProblemGroupDTO> data = _problemGroupService.GetByParent(parentID);
                ResponseModel<List<ProblemGroupDTO>> response = new ResponseModel<List<ProblemGroupDTO>>()
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
