using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileAssesses;
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
    [SwaggerTag("API quản lý hồ sơ đánh giá")]
    [RsaAuthorize]
    public class HumanProfileAssessesAPI : ControllerBase
    {
        private readonly IHumanProfileAssessesService _iHumanProfileAssessesService;

        public HumanProfileAssessesAPI(IHumanProfileAssessesService iHumanProfileAssessesService)
        {
            _iHumanProfileAssessesService = iHumanProfileAssessesService;
        }


        /// <summary>
        /// Lấy toàn bộ danh sách hồ sơ đánh giá ( khong phan trang) theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileAssessesService.GetAllByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileAssessesDTO>> response = new ResponseModel<List<HumanProfileAssessesDTO>>()
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