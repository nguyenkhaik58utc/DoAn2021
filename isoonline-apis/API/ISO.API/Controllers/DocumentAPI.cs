using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ISO.API.Controllers
{
    /// <summary>
    /// API tài liệu
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RsaAuthorize]
    public class DocumentAPI : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentAPI(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Lấy danh sách tài liệu trong thư mục có phân trang
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetData([FromBody] RequestModel<DocumentReqModel> req)
        {
            try
            {
                var data = _documentService.GetData(req.Data.type, req.Data.folderID, req.Data.pageIndex, req.Data.pageSize, req.Data.userID, req.Data.employeeID);
                ResponseModel<List<DocumentList>> response = new ResponseModel<List<DocumentList>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data.lstDocument,
                    TotalRows = data.total
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
        /// Kiểm tra link download nhanh
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckQuickDownload(Guid fileID)
        {
            try
            {
                bool c = _documentService.CheckQuickDownload(fileID);
                ResponseModel<bool> response = new ResponseModel<bool>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = c
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
        /// Lưu quyền truy cập document theo user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SavePublicPermission(RequestModel<DocumentPublicSaveRequest> request)
        {
            try
            {
                var data = _documentService.SavePublicPermission(request.Data.DocumentID, request.Data.IsPublic);
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
