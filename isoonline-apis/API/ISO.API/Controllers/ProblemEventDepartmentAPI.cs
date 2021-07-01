using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [SwaggerTag("API liên quan đến gán phòng ban xử lý sự cố")]
    public class ProblemEventDepartmentAPI : ControllerBase
    {
        private readonly IProblemEventDepService _problemEventDepService;
        public ProblemEventDepartmentAPI(IProblemEventDepService problemEventDepService)
        {
            _problemEventDepService = problemEventDepService;
        }
        [HttpGet]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult GetByProblem(int problemID)
        {
            try
            {
                List<ProblemEventDepDTO> data = _problemEventDepService.GetByProblemEvent(problemID);
                ResponseModel<List<ProblemEventDepDTO>> response = new ResponseModel<List<ProblemEventDepDTO>>()
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
        [RsaAuthorize]
        public IActionResult Insert([FromBody] RequestModel<ProblemEventDepInsModel> requestModel)
        {
            try
            {
                int data = _problemEventDepService.Insert(requestModel.Data);
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
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult Delete([FromBody] RequestModel<ProblemEventDepDelModel> requestModel)
        {
            try
            {
                int data = _problemEventDepService.Delete(requestModel.Data);
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
        [HttpPost]
        [Route("[action]")]
        [RsaAuthorize]
        public IActionResult InsertUpdate([FromBody] RequestModel<ProblemEventDepInsModel> requestModel)
        {
            try
            {
                ProblemEventDepDelModel delModel = new ProblemEventDepDelModel()
                {
                    ID = requestModel.Data.ProblemEventID.Value,
                    UpdatedAt = requestModel.Data.CreatedAt,
                    UpdatedBy = requestModel.Data.CreatedBy
                };
                _problemEventDepService.DeleteByProblemEvent(delModel);
               
                List<ProblemEventDepInsModel2> models = new List<ProblemEventDepInsModel2>();
                foreach (var item in requestModel.Data.DepartmentIDs)
                {
                    ProblemEventDepInsModel2 model = new ProblemEventDepInsModel2()
                    {
                        CreatedAt = requestModel.Data.CreatedAt,
                        CreatedBy = requestModel.Data.CreatedBy,
                        ProblemEventID = requestModel.Data.ProblemEventID,
                        IsDelete = false,
                        UpdatedAt = requestModel.Data.UpdatedAt,
                        UpdatedBy = requestModel.Data.UpdatedBy
                    };
                    model.DepartmentID = item;
                    models.Add(model);
                }
                int data = _problemEventDepService.InsertMulti(models.ToArray());
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
