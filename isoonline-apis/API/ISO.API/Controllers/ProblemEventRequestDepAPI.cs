using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemEventRequestDepAPI : ControllerBase
    {
        private readonly IProblemEventRequestDepService _problemEventRequestDepService;
        public ProblemEventRequestDepAPI(IProblemEventRequestDepService problemEventRequestDepService)
        {
            _problemEventRequestDepService = problemEventRequestDepService;
        }
        
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetPaging([FromBody] RequestModel<ProblemEventRequestDepSearchModel> requestModel)
        {
            try
            {
                List<ProblemEventRequestDepDTO> data = _problemEventRequestDepService.GetPaging(out int totalRows, requestModel.Data);
                ResponseModel<List<ProblemEventRequestDepDTO>> response = new ResponseModel<List<ProblemEventRequestDepDTO>>()
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
        
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByID(int ID)
        {
            try
            {
                ProblemEventRequestDepDTO data = _problemEventRequestDepService.GetByID(ID);
                ResponseModel<ProblemEventRequestDepDTO> response = new ResponseModel<ProblemEventRequestDepDTO>()
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
        
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult InsertUpdate(RequestModel<ProblemEventRequestDepInsUpdModel> req)
        {
            try
            {
                int rs = _problemEventRequestDepService.InsertUpdate(req.Data);
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
        
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Delete(RequestModel<ProblemEventRequestDepDelModel> req)
        {
            try
            {
                int affectedRows = _problemEventRequestDepService.Delete(req.Data);
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
                        Message = "Đơn vị tiếp nhận đang được sửa dụng!",
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
    }
}
