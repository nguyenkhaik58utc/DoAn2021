using System;
using System.Collections;
using System.Collections.Generic;
using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace ISO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("API liên quan phòng ban")]
    public class HumanDepartmentAPI : ControllerBase
    {
        private readonly IHumanDepartmentService _humanDepartmentService;
        public HumanDepartmentAPI(IHumanDepartmentService humanDepartmentService)
        {
            _humanDepartmentService = humanDepartmentService;
        }

        /// <summary>Danh sách phòng ban</summary>
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
        public IActionResult GetAll([FromBody] RequestModel<HumanDepartmentReqModel> requestModel)
        {
            try
            {
                List<HumanDepartmentDTO> data = _humanDepartmentService.GetAll();
                ResponseModel<List<HumanDepartmentDTO>> response = new ResponseModel<List<HumanDepartmentDTO>>()
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
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByID(int depID)
        {
            try
            {
                HumanDepartmentDTO data = _humanDepartmentService.GetByID(depID);
                ResponseModel<HumanDepartmentDTO> response = new ResponseModel<HumanDepartmentDTO>()
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
