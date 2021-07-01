using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileContract;
using ISO.API.Models.HumanProfileReward;
using ISO.API.Models.HumanProfileWorkExperience;
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
    [SwaggerTag("API quản lý hồ sơ quá trình công tác")]
   // [RsaAuthorize]
    public class HumanProfileWorkExperienceAPI : ControllerBase
    {
        private readonly IHumanProfileWorkExperienceService _iHumanProfileWorkExperienceService;

        public HumanProfileWorkExperienceAPI(IHumanProfileWorkExperienceService humanProfileWorkExperienceService)
        {
            _iHumanProfileWorkExperienceService = humanProfileWorkExperienceService;
        }


        /// <summary>
        /// Lấy danh sách hồ sơ theo ID nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByListEmployeeID(string lstEmployeeID)
        {
            try
            {
                var data = _iHumanProfileWorkExperienceService.GetListByListEmployeeID(lstEmployeeID);
                ResponseModel<List<HumanProfileWorkExperienceDTO>> response = new ResponseModel<List<HumanProfileWorkExperienceDTO>>()
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
        /// <summary>
        /// Lấy danh sách hồ sơ theo ID nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileWorkExperienceService.GetListByEmployeeID(employeeID);
                ResponseModel<List<HumanProfileWorkExperienceDTO>> response = new ResponseModel<List<HumanProfileWorkExperienceDTO>>()
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
