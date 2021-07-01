using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ISO.API.Controllers
{
    /// <summary>
    /// API máy chấm công
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RsaAuthorize]
    public class TimekeeperAPI : ControllerBase
    {
        private readonly ITimekeeperService _timekeeperService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timekeeperService"></param>
        public TimekeeperAPI(ITimekeeperService timekeeperService)
        {
            _timekeeperService = timekeeperService;
        }

        /// <summary>
        /// Lấy danh sách máy chấm công
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetData()
        {
            try
            {
                var data = _timekeeperService.GetData();
                ResponseModel<List<TimekeeperDTO>> response = new ResponseModel<List<TimekeeperDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data.lstTimekeeper
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
        /// Lấy chi tiết thông tin máy chấm công
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByID(int ID)
        {
            try
            {
                var data = _timekeeperService.GetByID(ID);
                ResponseModel<TimekeeperDTO> response = new ResponseModel<TimekeeperDTO>()
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
        /// Thêm mới máy chấm công
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert(RequestModel<TimekeeperDTO> req)
        {
            try
            {
                int rs = _timekeeperService.Insert(req.Data);
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
        /// Cập nhật thông tin máy chấm công
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(RequestModel<TimekeeperDTO> req)
        {
            try
            {
                var rs = _timekeeperService.Update(req.Data);
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
        /// Xóa máy chấm công
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(RequestModel<TimekeeperReqModel> req)
        {
            try
            {
                var rs = _timekeeperService.Delete(req.Data.ID);
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
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        //public IActionResult Inputdatalog(int idTimkeeper, string IP, int Post)
        public IActionResult InputDataLog(RequestModel<TimekeeperDTO> req)
        {
            //string error;
            //return this.Store(managerTimkeeperService.InputTimekeeping(idTimkeeper, IP, Post, out error));

            try
            {
                string error = string.Empty;
                var rs = _timekeeperService.InputTimekeeping(req.Data.ID, req.Data.IP, req.Data.Post, out error);
                if (rs)
                {
                    ResponseModel<string> response = new ResponseModel<string>()
                    {
                        MessageCode = "0000",
                        Message = "Success!",
                        Data = "Lấy dữ liệu thành công"
                    };
                    return Ok(response);
                }
                else
                {
                    ResponseModel<string> response = new ResponseModel<string>()
                    {
                        MessageCode = "0001",
                        Message = error,
                        Data = "Không lấy được dữ liệu"
                    };
                    return Ok(response);
                }    
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
