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
    [SwaggerTag("API liên quan đến Module hệ thống")]
    public class BusinessModuleAPI : ControllerBase
    {
        private readonly IBusinessModuleService _businessModuleService;
        public BusinessModuleAPI(IBusinessModuleService businessModuleService)
        {
            _businessModuleService = businessModuleService;
        }
        [HttpPost]
        [RsaAuthorize]
        [Route("[action]")]
        public IActionResult GetPaging([FromBody] RequestModel<string> requestModel)
        {
            try
            {
                BusinessModuleReqModel businessModuleReqModel = new BusinessModuleReqModel();
                List<BusinessModuleDTO> data = _businessModuleService.GetPaging(out int totalRows, businessModuleReqModel);
                ResponseModel<List<BusinessModuleDTO>> response = new ResponseModel<List<BusinessModuleDTO>>()
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
    }
}
