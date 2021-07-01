using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RsaAuthorize]
    [SwaggerTag("API liên quan đến chức danh của phòng ban")]
    public class DepartmentTitleAPI : ControllerBase
    {
        private readonly IDepartmentTitleService _departmentTitleService;
        private readonly ILogger<DepartmentTitleAPI> _logger;

        public DepartmentTitleAPI(IDepartmentTitleService departmentTitleService, ILogger<DepartmentTitleAPI> logger)
        {
            _departmentTitleService = departmentTitleService;
            _logger = logger;
        }
        ///<summary>Danh sách Chức danh theo phòng ban</summary>
        ///<remarks></remarks>
        ///<param name="requestModel"></param>
        ///<returns></returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByDepartment([FromBody] RequestModel<DepartmentTitleReqModel> requestModel)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(requestModel));
            try
            {
                List<DepartmentTitleDTO> data = _departmentTitleService.GetByDepartment(requestModel.Data.DepartmentID);
                ResponseModel<List<DepartmentTitleDTO>> response = new ResponseModel<List<DepartmentTitleDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
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
        /// Lấy danh sách chức danh được gán cho phòng ban theo chức danh
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByTitle(int titleID)
        {
            try
            {
                List<DepartmentTitleDTO> data = _departmentTitleService.GetByTitle(titleID);
                ResponseModel<List<DepartmentTitleDTO>> response = new ResponseModel<List<DepartmentTitleDTO>>()
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
        /// Gán chức danh cho phòng ban
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Insert([FromBody] RequestModel<DepartmentTitleDTO> requestModel)
        {
            try
            {
                int rs = _departmentTitleService.Insert(requestModel.Data);
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

        /// <summary>
        /// Bỏ gán chức danh cho phòng ban
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Delete(RequestModel<DepartmentTitleReqModel> req)
        {
            try
            {
                var rs = _departmentTitleService.Delete(req.Data.ID.Value);

                ResponseModel<int> response = new ResponseModel<int>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = 1
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
        /// Lấy danh sách chức danh được gán cho phòng ban và user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByDepAndEmp(int depID, int empID)
        {
            try
            {
                List<DepartmentTitleGetByDepEmpModel> data = _departmentTitleService.GetByDepAndEmp(depID, empID);
                ResponseModel<List<DepartmentTitleGetByDepEmpModel>> response = new ResponseModel<List<DepartmentTitleGetByDepEmpModel>>()
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
