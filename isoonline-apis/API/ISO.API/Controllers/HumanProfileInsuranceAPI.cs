using ISO.API.Middleware;
using ISO.API.Models;
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
    [SwaggerTag("API quản lý hồ sơ bảo hiểm")]
    [RsaAuthorize]
    public class HumanProfileInsuranceAPI : ControllerBase
    {
        private readonly IHumanProfileInsuranceService _iHumanProfileInsuranceService;

        public HumanProfileInsuranceAPI(IHumanProfileInsuranceService iHumanProfileInsuranceService)
        {
            _iHumanProfileInsuranceService = iHumanProfileInsuranceService;
        }


        /// <summary>
        /// Lấy toàn bộ danh sách hồ sơ bảo hiểm ( khong phan trang) theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileInsuranceService.GetAllByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileInsurancesDTO>> response = new ResponseModel<List<HumanProfileInsurancesDTO>>()
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