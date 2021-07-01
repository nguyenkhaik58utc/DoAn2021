using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.EducationResult;
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
    [SwaggerTag("API quản lý danh mục kết quả đào tạo")]
    [RsaAuthorize]

    public class EducationResultAPI : ControllerBase
    {
        private readonly IEducationResultService _iEducationResultService;

        public EducationResultAPI(IEducationResultService iEducationResultService)
        {
            _iEducationResultService = iEducationResultService;
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
                var data = _iEducationResultService.GetByID(ID);
                ResponseModel<EducationResultDTO> response = new ResponseModel<EducationResultDTO>()
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
        public IActionResult GetAll()
        {
            try
            {
                var data = _iEducationResultService.GetAll();
                ResponseModel<List<EducationResultDTO>> response = new ResponseModel<List<EducationResultDTO>>()
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
        public IActionResult Insert([FromBody] RequestModel<EducationResultDTO> request)
        {
            try
            {
                var data = _iEducationResultService.Insert(request.Data);
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
        public IActionResult Update([FromBody] RequestModel<EducationResultDTO> request)
        {
            try
            {
                var data = _iEducationResultService.Update(request.Data);
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
                var data = _iEducationResultService.Delete(request.Data.ID);
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