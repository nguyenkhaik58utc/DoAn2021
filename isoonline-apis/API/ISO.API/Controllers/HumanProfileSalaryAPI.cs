using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileCertificate;
using ISO.API.Models.HumanProfileInsurances;
using ISO.API.Models.HumanProfileSalary;
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
    [SwaggerTag("API quản lý hồ sơ lương")]
    [RsaAuthorize]
    public class HumanProfileSalaryAPI : ControllerBase
    {
        private readonly IHumanProfileSalaryService _iHumanProfileSalaryService;

        public HumanProfileSalaryAPI(IHumanProfileSalaryService iHumanProfileSalaryService)
        {
            _iHumanProfileSalaryService = iHumanProfileSalaryService;
        }


        /// <summary>
        /// Lấy toàn bộ danh sách hồ sơ lương ( khong phan trang) theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileSalaryService.GetAllByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileSalaryDTO>> response = new ResponseModel<List<HumanProfileSalaryDTO>>()
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