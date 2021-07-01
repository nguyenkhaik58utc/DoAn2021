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
    [SwaggerTag("API liên quan gán chức danh người dùng")]
    public class DepartmentTitleUserAPI : ControllerBase
    {
        private readonly IDepartmentTitleUserService _departmentTitleUserService;
        public DepartmentTitleUserAPI(IDepartmentTitleUserService departmentTitleUserService)
        {
            _departmentTitleUserService = departmentTitleUserService;
        }
        /// <summary>
        /// Thêm mới/Cập nhật gán chức danh cho user
        /// </summary>
        [HttpPost]
        [RsaAuthorize]
        [Route("[action]")]
        public IActionResult InsertUpdate([FromBody] RequestModel<DepartmentTitleUserInsUpdModel> requestModel)
        {
            try
            {
                int rs = _departmentTitleUserService.InsertUpdate(requestModel.Data);
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
        /// Lấy danh sách title gán với user
        /// </summary>
        [HttpGet]
        [RsaAuthorize]
        [Route("[action]")]
        public IActionResult GetTitleOfEmployee(int employeeID)
        {
            try
            {
                var data = _departmentTitleUserService.GetByEmployee(employeeID);
                ResponseModel<List<DepartmentTitleUserDTO>> response = new ResponseModel<List<DepartmentTitleUserDTO>>()
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
        /// Xoá gán chức danh và user
        /// </summary>
        [HttpPost]
        [RsaAuthorize]
        [Route("[action]")]
        public IActionResult DeleteByDepartmentTitleAndUser([FromBody]RequestModel<DepartmentTitleUserDeleteReqModel> requestModel)
        {
            try
            {
                int rs = _departmentTitleUserService.DeleteByDepartmentTitleAndUser(requestModel.Data.DepartmentTitleID, requestModel.Data.HumanEmployeeID, requestModel.Data.UpdatedBy, requestModel.Data.UpdatedAt);
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
    }
}
