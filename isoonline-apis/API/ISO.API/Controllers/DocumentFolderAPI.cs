using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.DocumentFolder;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace ISO.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [SwaggerTag("API quản lý Thư mục tài liệu")]
    [RsaAuthorize]
    public class DocumentFolderAPI : ControllerBase
    {
        private readonly IDocumentFolderService _iDocumentFolderService;

        public DocumentFolderAPI (IDocumentFolderService documentFolderService)
        {
            _iDocumentFolderService = documentFolderService;
        }

        /// <summary>
        /// Lấy danh mục  theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByID(int ID)
        {
            try
            {
                var data = _iDocumentFolderService.GetByID(ID);
                ResponseModel<DocumentFolderDTO> response = new ResponseModel<DocumentFolderDTO>()
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
        /// Lấy danh mục  theo Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByName(string Name,bool DocumentOut = false,int ParentID = 0)
        {
            try
            {
                var data = _iDocumentFolderService.GetByName(Name,DocumentOut,ParentID);
                ResponseModel<DocumentFolderDTO> response = new ResponseModel<DocumentFolderDTO>()
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
        /// Lay toàn bộ danh mục
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll(int DocumentOut)
        {
            try
            {
                var data = _iDocumentFolderService.GetAll(DocumentOut);
                ResponseModel<List<DocumentFolderDTO>> response = new ResponseModel<List<DocumentFolderDTO>>()
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
        /// Thêm mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert([FromBody] RequestModel<DocumentFolderDTO> request)
        {
            try
            {
                var data = _iDocumentFolderService.Insert(request.Data);
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
        /// cập nhật 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Update([FromBody] RequestModel<DocumentFolderDTO> request)
        {
            try
            {
                var data = _iDocumentFolderService.Update(request.Data);
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
        /// Xóa danh mục 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete([FromBody] RequestModel<DeleteRequest> request)
        {
            try
            {
                var data = _iDocumentFolderService.Delete(request.Data.ID);
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
