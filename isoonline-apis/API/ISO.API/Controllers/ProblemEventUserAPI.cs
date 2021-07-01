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
    [SwaggerTag("API liên quan người liên quan sự cố, sự kiện")]
    public class ProblemEventUserAPI : ControllerBase
    {
        private readonly IProblemEventUserService _problemEventUserService;
        public ProblemEventUserAPI(IProblemEventUserService problemEventUserService)
        {
            _problemEventUserService = problemEventUserService;
        }
        /// <summary>
        /// Lấy danh sách người liên quan theo ID sự cố, sự kiện
        /// </summary>
        /// <param name="problemEventID">ID sự cố, sự kiện</param>
        /// <returns>Danh sách người liên quan</returns>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByProblemEvent(int problemEventID, string listType)
        {
            try
            {
                List<ProblemEventUserDTO> data = _problemEventUserService.GetByProblemEvent(problemEventID, listType);
                ResponseModel<List<ProblemEventUserDTO>> response = new ResponseModel<List<ProblemEventUserDTO>>()
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
        /// Thêm người liên quan đến sự cố, sự kiện
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult InsertUpdate(RequestModel<ProblemRelativePeopleInsReqModelDTO> requestModel)
        {


            try
            {
                var rs = _problemEventUserService.InsertUpdate(requestModel.Data);
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
        /// Xoá người dùng liên quan sự cố sự kiện
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Delete(RequestModel<string> req)
        {
            try
            {
                var rs = _problemEventUserService.Delete(req.Data);
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
        /// Xoá nhiều người dùng liên quan sự cố sự kiện
        /// </summary>
        /// <param>-
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult DeleteMulti([FromBody] RequestModel<ProblemEventUserDelModel> requestModel)
        {
            try
            {
                int data = _problemEventUserService.DeleteMulti(requestModel.Data);
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

        /// <summary>
        /// Lấy danh sách người xử lý sự cố
        /// </summary>
        /// <param name="problemEventID">ID sự cố</param>
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetUserFix(int problemEventID)
        {
            try
            {
                List<ProblemUserFix> data = _problemEventUserService.GetUserFix(problemEventID);
                ResponseModel<List<ProblemUserFix>> response = new ResponseModel<List<ProblemUserFix>>()
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
        /// Lấy danh sách chức danh, tên phòng ban, tên người từ danh sách HumanEmployeeID
        /// </summary>
        /// <param name="lstHumanEmployeeIDs">list HumanEmployeeID</param>
        /// <returns>Danh sách người liên quan</returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetListRoleDepartmentFromListHumanEmployeeID([FromBody] RequestModel<BusinesslstHumanEmployeeIDs> requestModel)
        {
            try
            {
                List<HumanRoleHumanDepartmentHumanEmployeeDTO> data = _problemEventUserService.GetListRoleDepartmentFromListHumanEmployeeID(requestModel.Data.lstHumanEmployeeIDs);
                ResponseModel<List<HumanRoleHumanDepartmentHumanEmployeeDTO>> response = new ResponseModel<List<HumanRoleHumanDepartmentHumanEmployeeDTO>>()
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
