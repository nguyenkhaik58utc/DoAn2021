using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.HumanProfileCuriculmViate;
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
    [SwaggerTag("API quản lý hồ sơ lý lịch nhân viên")]
    [RsaAuthorize]
    public class HumanProfileCuriculmViateAPI : ControllerBase
    {
        private readonly IHumanProfileCuriculmViateService _iHumanProfileCuriculmViateService;

        public HumanProfileCuriculmViateAPI(IHumanProfileCuriculmViateService iHumanProfileCuriculmViateService)
        {
            _iHumanProfileCuriculmViateService = iHumanProfileCuriculmViateService;
        }

        /// <summary>
        /// Lấy hồ sơ lý lịch theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByHumanEmployeeID(int ID)
        {
            try
            {
                var data = _iHumanProfileCuriculmViateService.GetByHumanEmployeeID(ID);
                ResponseModel<HumanProfileCuriculmViateDTO> response = new ResponseModel<HumanProfileCuriculmViateDTO>()
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
        /// Lấy hồ sơ lý lịch theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFullByHumanEmployeeID(int ID)
        {
            try
            {
                var data = _iHumanProfileCuriculmViateService.GetFullByHumanEmployeeID(ID);
                ResponseModel<HumanProfileCuriculmViateReportDTO> response = new ResponseModel<HumanProfileCuriculmViateReportDTO>()
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
        /// Lấy danh sách thông tin chung theo danh sách id nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHumanCommonInformationByListEmployeeID(string lstID)
        {
            try
            {
                var data = _iHumanProfileCuriculmViateService.GetCommonIformationByListEmployeeID(lstID);
                ResponseModel<List<HumanCommonInformationDTO>> response = new ResponseModel<List<HumanCommonInformationDTO>>()
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
        /// Lấy danh sách thông tin lý lịch chính trị theo danh sách id nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHumanPoliticInformationByListEmployeeID(string lstID)
        {
            try
            {
                var data = _iHumanProfileCuriculmViateService.GetPoliticIformationByListEmployeeID(lstID);
                ResponseModel<List<HumanPoliticInformationDTO>> response = new ResponseModel<List<HumanPoliticInformationDTO>>()
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
        /// Lấy danh sách hồ sơ lý lịch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var data = _iHumanProfileCuriculmViateService.GetAll();
                ResponseModel<List<HumanProfileCuriculmViateDTO>> response = new ResponseModel<List<HumanProfileCuriculmViateDTO>>()
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
        /// Lưu hồ sơ lý lịch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Save([FromBody] RequestModel<HumanProfileCuriculmViateDTO> request)
        {
            try
            {
                var data = _iHumanProfileCuriculmViateService.Save(request.Data);
                ResponseModel<int> response = new ResponseModel<int>()
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