using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ISO.API.Models;
using ISO.API.Models.Position;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionAPI : ControllerBase
    {
        private readonly IPositionService _positionService;
        public PositionAPI(IPositionService positionService)
        {
            _positionService = positionService;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAll()
        {
            try
            {
                List<PositionDTO> data = _positionService.GetAll();
                ResponseModel<List<PositionDTO>> response = new ResponseModel<List<PositionDTO>>()
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
        public string Create(PositionDTO positionDTO)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("PositionName", positionDTO.PositionName);
                p.Add("PositionCode", positionDTO.PositionCode);
                p.Add("ManagementLevelID", positionDTO.ManagementLevelID);
                p.Add("IsActive", positionDTO.IsActive);
                p.Add("IsDelete", positionDTO.IsDelete);
                p.Add("CreateAt", positionDTO.CreateAt);
                p.Add("CreateBy", positionDTO.CreateBy);
                int affectedRows = _positionService.AddPosition(p);
                return affectedRows.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPut]
        [Route("[action]")]
        public string Update(PositionDTO positionDTO)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("ID", positionDTO.ID);
                p.Add("PositionName", positionDTO.PositionName);
                p.Add("PositionCode", positionDTO.PositionCode);
                p.Add("ManagementLevelID", positionDTO.ManagementLevelID);
                p.Add("IsActive", positionDTO.IsActive);
                p.Add("IsDelete", positionDTO.IsDelete);
                p.Add("UpdateAt", positionDTO.CreateAt);
                p.Add("UpdateBy", positionDTO.CreateBy);
                int affectedRows = _positionService.UpdatePosition(p);
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
                List<PositionDTO> data = _positionService.GetByID(ID);
                ResponseModel<List<PositionDTO>> response = new ResponseModel<List<PositionDTO>>()
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
                int rs = _positionService.deletePosition(ID);
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

    }
}

