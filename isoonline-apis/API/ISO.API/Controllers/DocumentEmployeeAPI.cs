using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.Document;
using ISO.API.Models.HumanEmployee;
using ISO.API.Models.ProfileCuriculmDeparmentTitle;
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
    [SwaggerTag("API Phân quyền truy cập document theo user")]
    [RsaAuthorize]
    public class DocumentEmployeeAPI : ControllerBase
    {
        private readonly IDocumentEmployeeService _documentEmployeeService;
        public DocumentEmployeeAPI(IDocumentEmployeeService documentEmployeeService)
        {
            _documentEmployeeService = documentEmployeeService;
        }

        /// <summary>
        /// Lưu quyền truy cập document theo user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SavePermission(RequestModel<DocumentEmployeeSaveRequest> request)
        {
            try
            {
                var data = _documentEmployeeService.SavePermission(request.Data.DocumentID, request.Data.EmployeeList);
                ResponseModel<bool> response = new ResponseModel<bool>()
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
        /// Lấy danh sách user đã được cấp quyền
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListByDocumentID(int documentID)
        {
            try
            {
                var data = _documentEmployeeService.GetListByDocumentID(documentID);
                ResponseModel<List<HumanEmployeeResponse>> response = new ResponseModel<List<HumanEmployeeResponse>>()
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
