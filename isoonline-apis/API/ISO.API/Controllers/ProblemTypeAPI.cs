using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
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
    [SwaggerTag("API liên quan đến Loại sự cố, sự kiện")]
    public class ProblemTypeAPI : ControllerBase
    {
        private readonly IProblemTypeService _typeService;
        public ProblemTypeAPI(IProblemTypeService typeService)
        {
            _typeService = typeService;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAll()
        {
            try
            {
                List<ProblemTypeDTO> data = _typeService.GetAll();
                ResponseModel<List<ProblemTypeDTO>> response = new ResponseModel<List<ProblemTypeDTO>>()
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

        [HttpPost]
        [Route("[action]")]
        public string Create(ProblemTypeDTO problemTypeDTO)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("ProblemTypeName", problemTypeDTO.ProblemTypeName);
                p.Add("Code", problemTypeDTO.Code);
                p.Add("Description", problemTypeDTO.Description);
                p.Add("IsDelete", problemTypeDTO.IsDelete);
                p.Add("CreateAt", problemTypeDTO.CreateAt);
                p.Add("CreateBy", problemTypeDTO.CreateBy);
                int affectedRows = _typeService.Add(p);
                return affectedRows.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPut]
        [Route("[action]")]
        public string Update(ProblemTypeDTO problemTypeDTO)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("ID", problemTypeDTO.ID);
                p.Add("ProblemTypeName", problemTypeDTO.ProblemTypeName);
                p.Add("Code", problemTypeDTO.Code);
                p.Add("Description", problemTypeDTO.Description);
                p.Add("IsDelete", problemTypeDTO.IsDelete);
                p.Add("UpdateAt", problemTypeDTO.CreateAt);
                p.Add("UpdateBy", problemTypeDTO.CreateBy);
                int affectedRows = _typeService.Update(p);
                return affectedRows.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetByID(int ID)
        {
            try
            {
                List<ProblemTypeDTO> data = _typeService.GetByID(ID);
                ResponseModel<List<ProblemTypeDTO>> response = new ResponseModel<List<ProblemTypeDTO>>()
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
        [Route("[action]")]
        public IActionResult GetListByID(int ID)
        {
            try
            {
                List<ProblemTypeDTO> data = _typeService.GetListByID(ID);
                ResponseModel<List<ProblemTypeDTO>> response = new ResponseModel<List<ProblemTypeDTO>>()
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

        [HttpDelete]
        [Route("[action]")]
        public IActionResult Delete(int ID)
        {
            try
            {
                int affectedRows = _typeService.Delete(ID);
                if (affectedRows != 0)
                {
                    ResponseModel<int> response = new ResponseModel<int>()
                    {
                        MessageCode = "0000",
                        Message = "Success!",
                        Data = affectedRows
                    };
                    return Ok(response);
                }
                else
                {
                    ResponseModel<int> response = new ResponseModel<int>()
                    {
                        MessageCode = "0000",
                        Message = "Loại sự cố đang được sửa dụng!",
                        Data = affectedRows
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

        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Default([FromBody] RequestModel<ProblemTypeDTO> requestModel)
        {
            try
            {
                int affectedRows = _typeService.Default(requestModel.Data.ID);
                ResponseModel<int> response = new ResponseModel<int>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = affectedRows
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
