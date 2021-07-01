using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileAttachment;
using ISO.API.Models.HumanProfileCertificate;
using ISO.API.Models.HumanProfileInsurances;
using ISO.API.Models.ProfileHumanPermission;
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
    [SwaggerTag("API quản lý hồ sơ đính kèm")]
    [RsaAuthorize]
    public class HumanProfileAttachmentAPI : ControllerBase
    {
        private readonly IHumanProfileAttachmentService _iHumanProfileAttachmentService;

        public HumanProfileAttachmentAPI(IHumanProfileAttachmentService iHumanProfileAttachmentService)
        {
            _iHumanProfileAttachmentService = iHumanProfileAttachmentService;
        }


        /// <summary>
        /// Lấy toàn bộ danh sách hồ sơ đính kèm ( khong phan trang) theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileAttachmentService.GetAllByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileAttachmentDTO>> response = new ResponseModel<List<HumanProfileAttachmentDTO>>()
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