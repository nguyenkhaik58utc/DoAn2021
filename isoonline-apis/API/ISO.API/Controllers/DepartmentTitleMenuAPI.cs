using System;
using System.Collections.Generic;
using Dapper;
using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ISO.API.Controllers
{
    [Route("api/DepartmentTitleMenuAPI")]
    [ApiController]
    [SwaggerTag("API liên quan đến chức danh của phòng ban và menu")]
    public class DepartmentTitleMenuAPI : ControllerBase
    {
        private readonly IDepartmentTitleMenuService _departmentTitleMenuService;
        public DepartmentTitleMenuAPI(IDepartmentTitleMenuService departmentTitleMenuService)
        {
            _departmentTitleMenuService = departmentTitleMenuService;
        }
        ///<summary>Danh sách Menu theo phòng ban và chức danh</summary>
        ///<remarks></remarks>
        ///<param name="requestModel"></param>
        ///<returns></returns>
        [HttpPost]
        [Route("GetMenuByDepartmentAndTitle")]
        [RsaAuthorize]
        public IActionResult GetMenuByDepartmentAndTitle([FromBody] RequestModel<DepartmentTitleMenuReqModel> requestModel)
        {
            try
            {
                List<DepartmentTitleMenuDTO> data = _departmentTitleMenuService.GetByDepartmentAndTitle(requestModel.Data.DepartmentID, requestModel.Data.TitleID);
                ResponseModel<List<DepartmentTitleMenuDTO>> response = new ResponseModel<List<DepartmentTitleMenuDTO>>()
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
        ///<summary>Gán Menu với chức danh phòng ban</summary>
        ///<remarks></remarks>
        ///<param name="requestModel"></param>
        ///<returns></returns>
        [HttpPost]
        [Route("InsertDepartmentTitleMenu")]
        [RsaAuthorize]
        public IActionResult Insert([FromBody] RequestModel<DepartmentTitleMenuInsModel> requestModel)
        {
            try
            {
                int rs = _departmentTitleMenuService.Insert(requestModel.Data);
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

        ///<summary>Danh sách Menu của 1 user cụ thể</summary>
        ///<remarks></remarks>
        ///<param name="userID">User cần lấy menu</param>
        ///<returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetMenuOfUser(int userID)
        {
            try
            {
                List<DepartmentTitleMenuDTO> data = _departmentTitleMenuService.GetByUser(userID);
                ResponseModel<List<DepartmentTitleMenuDTO>> response = new ResponseModel<List<DepartmentTitleMenuDTO>>()
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

        // HungNM
        ///<summary>Toan bo bang Menu theo cau truc cay cha con</summary>
        ///<remarks></remarks>
        ///<returns></returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetAllTreeRollMenuV3()
        {
            try
            {
                List<TreeRollMenuV3DTO> data = _departmentTitleMenuService.GetAllTreeRollMenuV3();
                ResponseModel<List<TreeRollMenuV3DTO>> response = new ResponseModel<List<TreeRollMenuV3DTO>>()
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

        ///<summary>Luu phan quyen menu cua user theo chuc danh</summary>
        ///<remarks></remarks>
        ///<returns></returns>
        [HttpPut]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult UpdateMenuRoleDepartmentTitle(RequestModel<CRUDDepartmentTitleMenuV3DTO> req)
        {
            try
            {
                var rs = _departmentTitleMenuService.UpdateDepartmentTitleMenuV3(req.Data);
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

        ///<summary>Copy quyen cua user trong phan quyen menu theo chuc danh</summary>
        ///<remarks></remarks>
        ///<returns></returns>
        [HttpPut]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult CopyMenuTitleRoleV3(RequestModel<CopyMenuTitleRoleV3DTO> req)
        {
            try
            {
                var rs = _departmentTitleMenuService.CopyMenuTitleRoleV3(req.Data);
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


        // End. HungNM.
    }

}
