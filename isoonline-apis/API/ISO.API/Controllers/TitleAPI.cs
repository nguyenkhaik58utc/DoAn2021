using Dapper;
using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ISO.API.Controllers
{
    /// <summary>
    /// API chức danh
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RsaAuthorize]
    public class TitleAPI : ControllerBase
    {
        private readonly ITitleService _titleService;
        private readonly ILogger<DepartmentTitleAPI> _logger;

        public TitleAPI(ITitleService titleService, ILogger<DepartmentTitleAPI> logger)
        {
            _titleService = titleService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách chức danh có phân trang
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetData([FromBody] RequestModel<TitleReqModel> req)
        {
            try
            {
                var data = _titleService.GetData(req.Data.pageIndex, req.Data.pageSize);
                ResponseModel<List<TitleDTO>> response = new ResponseModel<List<TitleDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data.lstTitle,
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
        /// Lấy chi tiết chức danh
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByID(int ID)
        {
            try
            {
                var data = _titleService.GetByID(ID);
                ResponseModel<TitleDTO> response = new ResponseModel<TitleDTO>()
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
        /// Thêm mới chức danh
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert(RequestModel<TitleDTO> req)
        {
            try
            {
                int rs = _titleService.Insert(req.Data);
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
        /// Cập nhật thông tin chức danh
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(RequestModel<TitleDTO> req)
        {
            try
            {
                var rs = _titleService.Update(req.Data);
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
        /// Xóa chức danh
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(RequestModel<TitleReqModel> req)
        {
            try
            {
                var rs = _titleService.Delete(req.Data.ID);
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
        /// Lấy danh sách chức vụ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetComboPosition()
        {
            try
            {
                var data = _titleService.GetComboPosition();
                ResponseModel<List<PositionCboDTO>> response = new ResponseModel<List<PositionCboDTO>>()
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

        [HttpGet]
        public IActionResult GetNotAsDepartment(int departmentID)
        {
            try
            {
                var data = _titleService.GetNotAsDepartment(departmentID);
                ResponseModel<List<TitleDTO>> response = new ResponseModel<List<TitleDTO>>()
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
