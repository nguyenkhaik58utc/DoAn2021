using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.Document;
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
    [SwaggerTag("API Phân quyền truy cập document theo phòng ban")]
    [RsaAuthorize]
    public class DocumentDepartmentAPI : ControllerBase
    {
        private readonly IDocumentDepartmentService _documentDepartmentService;
        public DocumentDepartmentAPI(IDocumentDepartmentService documentDepartmentService)
        {
            _documentDepartmentService = documentDepartmentService;
        }

        /// <summary>
        /// Lưu quyền truy cập document theo phòng ban
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SavePermission(RequestModel<DocumentDepartmentSaveRequest> request)
        {
            try
            {
                var data = _documentDepartmentService.SavePermission(request.Data.DocumentID, request.Data.DepartmentIDList);
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
        /// Lấy danh sách phòng ban đã được cấp quyền
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListByDocumentID(int documentID)
        {
            try
            {
                var data = _documentDepartmentService.GetListByDocumentID(documentID);
                ResponseModel<List<DocumentDepartmentDTO>> response = new ResponseModel<List<DocumentDepartmentDTO>>()
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
