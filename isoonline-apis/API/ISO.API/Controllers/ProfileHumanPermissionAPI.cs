using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.ProfileCuriculmDeparmentTitle;
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
    [SwaggerTag("API Phân quyền thao tác hồ sơ lý lịch")]
    [RsaAuthorize]
    public class ProfileHumanPermissionAPI : ControllerBase
    {
        private readonly IProfileHumanPermissionService _profileHumanPermissionService;

        public ProfileHumanPermissionAPI(IProfileHumanPermissionService profileHumanPermissionService)
        {
            _profileHumanPermissionService = profileHumanPermissionService;
        }
       
        /// <summary>
        /// Kiểm tra quyền thao tác hồ sơ lý lịch theo HumanEmployeeID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckProfileHumanPermission(RequestModel<CheckProfileHumanPermissionRequest> request)
        {
            try
            {
                var data = _profileHumanPermissionService.CheckProfielHumanPermission(request.Data);
                ResponseModel<List<CheckProfileHumanPermissionResponse>> response = new ResponseModel<List<CheckProfileHumanPermissionResponse>>()
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
        /// Lay toàn bộ quyền thao tác hồ sơ theo chức danh
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAllByDepartmentTitle(RequestModel<GetAllPermissionRequest> request)
        {
            try
            {
                var data = _profileHumanPermissionService.GetAllByDepartmentTitle(request.Data.DepartmentTitleFromID);
                ResponseModel<List<ProfilePermissionResponse>> response = new ResponseModel<List<ProfilePermissionResponse>>()
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
        /// Lưu lại quyền thao tác hồ sơ lý lịch theo chức danh
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveProfilePermission([FromBody]RequestModel<SaveProfilePermissionRequest> request)
        {
            try
            {
                var data = _profileHumanPermissionService.SaveProfilePermission(request.Data);
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