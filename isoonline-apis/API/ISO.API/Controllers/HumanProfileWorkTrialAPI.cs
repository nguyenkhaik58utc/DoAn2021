using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileCertificate;
using ISO.API.Models.HumanProfileInsurances;
using ISO.API.Models.HumanProfileSalary;
using ISO.API.Models.HumanProfileWorkTrial;
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
    [SwaggerTag("API quản lý hồ sơ thử việc")]
    [RsaAuthorize]
    public class HumanProfileWorkTrialAPI : ControllerBase
    {
        private readonly IHumanProfileWorkTrialService _iHumanProfileWorkTrialService;

        public HumanProfileWorkTrialAPI(IHumanProfileWorkTrialService iHumanProfileWorkTrialService)
        {
            _iHumanProfileWorkTrialService = iHumanProfileWorkTrialService;
        }


        /// <summary>
        /// Lấy toàn bộ danh sách hồ sơ thử việc ( khong phan trang) theo EmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllByEmployeeID(int employeeID)
        {
            try
            {
                var data = _iHumanProfileWorkTrialService.GetAllByEmployeeID(employeeID);
                HumanProfileWorkTrialDTO workTrialDTO = new HumanProfileWorkTrialDTO();
                if (data.Count > 0)
                {
                    workTrialDTO.HumanEmployeeID = data[0].HumanEmployeeID;
                    workTrialDTO.HumanEmployeeName = data[0].HumanEmployeeName;
                    workTrialDTO.FromDate = data[0].FromDate;
                    workTrialDTO.ToDate = data[0].ToDate;
                    workTrialDTO.IsAccept = data[0].IsAccept;
                    workTrialDTO.IsApproval = data[0].IsApproval;
                    workTrialDTO.IsEdit = data[0].IsEdit;
                    foreach (var item in data)
                    {
                        workTrialDTO.DepartmentTitle += item.DepartmentName + " -- " + item.TitleName + " </br>";
                    }
                }
                ResponseModel<HumanProfileWorkTrialDTO> response = new ResponseModel<HumanProfileWorkTrialDTO>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = workTrialDTO
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