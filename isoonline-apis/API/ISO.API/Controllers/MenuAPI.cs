using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models;
using ISO.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ISO.API.Middleware;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("API liên quan Menu")]

    public class MenuAPI : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuAPI(IMenuService menuService)
        {
            _menuService = menuService;
        }
        /// <summary>Danh sách Menu</summary>
        /// <remarks></remarks>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        [HttpPost]
        [Route("[action]")]
        public IActionResult GetAll([FromBody] RequestModel<MenuReqModel> requestModel)
        {
            try
            {
                List<MenuDTO> data = _menuService.GetAll();
                ResponseModel<List<MenuDTO>> response = new ResponseModel<List<MenuDTO>>()
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

        // HungNM.
        ///<summary>MenuTreeBusinessModule của 1 user cụ thể</summary>
        ///<remarks></remarks>
        ///<param name="userID">User cần lấy menu</param>
        ///<returns></returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetBusinessModule([FromBody] RequestModel<MenuBusinessModule> requestModel)
        {
            try
            {
                List<MenuDTO> data = _menuService.GetBusinessModule(requestModel.Data.userID);
                ResponseModel<List<MenuDTO>> response = new ResponseModel<List<MenuDTO>>()
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

        ///<summary>MenuTreeBusinessFunction của 1 user cụ thể</summary>
        ///<remarks></remarks>
        ///<param name="userID">User cần lấy menu</param>
        ///<returns></returns>
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetBusinessFunction([FromBody] RequestModel<MenuBusinessFunction> requestModel)
        {
            try
            {
                List<MenuDTO> data = _menuService.GetBusinessFunction(requestModel.Data.userID, requestModel.Data.ModuleCode);
                ResponseModel<List<MenuDTO>> response = new ResponseModel<List<MenuDTO>>()
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
        // End. HungNM.

    }
}
