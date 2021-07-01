using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("API liên quan đến mức độ nghiêm trọng sự cố, sự kiện")]
    public class ProblemCriticalLevelAPI : ControllerBase
    {
        private readonly IProblemCriticalLevelService problemCriticalLevelService;
        public ProblemCriticalLevelAPI(IProblemCriticalLevelService criticalLevelService)
        {
            problemCriticalLevelService = criticalLevelService;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAll()
        {
            try
            {
                List<ProblemCriticalLevelDTO> data = problemCriticalLevelService.GetAll();
                ResponseModel<List<ProblemCriticalLevelDTO>> response = new ResponseModel<List<ProblemCriticalLevelDTO>>()
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
        public string Create(ProblemCriticalLevelDTO problemCriticalLevelDTO)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("CriticalLevelName", problemCriticalLevelDTO.CriticalLevelName);
                p.Add("Code", problemCriticalLevelDTO.Code);
                p.Add("Description", problemCriticalLevelDTO.Description);
                p.Add("IsDelete", problemCriticalLevelDTO.IsDelete);
                p.Add("CreateAt", problemCriticalLevelDTO.CreateAt);
                p.Add("CreateBy", problemCriticalLevelDTO.CreateBy);
                p.Add("Color", problemCriticalLevelDTO.Color);
                int affectedRows = problemCriticalLevelService.AddProblemCriticalLevel(p);
                return affectedRows.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPut]
        [Route("[action]")]
        public string Update(ProblemCriticalLevelDTO problemCriticalLevelDTO)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("ID", problemCriticalLevelDTO.ID);
                p.Add("CriticalLevelName", problemCriticalLevelDTO.CriticalLevelName);
                p.Add("Code", problemCriticalLevelDTO.Code);
                p.Add("Description", problemCriticalLevelDTO.Description);
                p.Add("IsDelete", problemCriticalLevelDTO.IsDelete);
                p.Add("UpdateAt", problemCriticalLevelDTO.CreateAt);
                p.Add("UpdateBy", problemCriticalLevelDTO.CreateBy);
                p.Add("Color", problemCriticalLevelDTO.Color);
                int affectedRows = problemCriticalLevelService.UpdateProblemCriticalLevel(p);
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
                List<ProblemCriticalLevelDTO> data = problemCriticalLevelService.GetByID(ID);
                ResponseModel<List<ProblemCriticalLevelDTO>> response = new ResponseModel<List<ProblemCriticalLevelDTO>>()
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
                int affectedRows = problemCriticalLevelService.DeleteProblemCriticalLevel(ID);
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
                        Message = "Mức độ nghiêm trọng đang được sửa dụng!",
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
        public IActionResult Default([FromBody] RequestModel<ProblemCriticalLevelDTO> requestModel)
        {
            try
            {
                int affectedRows = problemCriticalLevelService.Default(requestModel.Data.ID);
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
