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
    /// API đăng ký làm OT
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RsaAuthorize]
    public class EmployeeOTAPI : ControllerBase
    {
        private readonly IEmployeeOTService _empOtService;

        public EmployeeOTAPI(IEmployeeOTService empOtService)
        {
            _empOtService = empOtService;
        }

        /// <summary>
        /// Lấy danh sách đăng ký OT có phân trang
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetData([FromBody] RequestModel<EmployeeOTReq> req)
        {
            try
            {
                var data = _empOtService.GetData(req.Data.pageIndex, req.Data.pageSize);
                ResponseModel<List<EmployeeOT>> response = new ResponseModel<List<EmployeeOT>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data.lstEmployee,
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
        /// Lấy chi tiết đăng ký OT
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByID(int ID)
        {
            try
            {
                var data = _empOtService.GetByID(ID);
                ResponseModel<EmployeeOT> response = new ResponseModel<EmployeeOT>()
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
        /// Thêm mới đăng ký OT
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert(RequestModel<EmployeeOT> req)
        {
            try
            {
                int rs = _empOtService.Insert(req.Data);
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
        /// Cập nhật thông tin đăng ký OT
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(RequestModel<EmployeeOT> req)
        {
            try
            {
                var rs = _empOtService.Update(req.Data);
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
        /// Xóa đăng ký OT
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(RequestModel<EmployeeOTReq> req)
        {
            try
            {
                var rs = _empOtService.Delete(req.Data.ID);
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
    }
}
