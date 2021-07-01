using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ISO.API.Models;
using ISO.API.Models.ManagementLevel;
using ISO.API.Models.Position;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ISO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementLevelAPI : ControllerBase
    {
        private readonly IManagementLevelService _managementLevelService;
        public ManagementLevelAPI(IManagementLevelService managementLevelService)
        {
            _managementLevelService = managementLevelService;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAll()
        {
            try
            {
                List<ManagementLevelDTO> data = _managementLevelService.GetAll();
                ResponseModel<List<ManagementLevelDTO>> response = new ResponseModel<List<ManagementLevelDTO>>()
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
        public string Create(ManagementLevelDTO managementLevel)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("ManagementLevelName", managementLevel.ManagementLevelName);
                p.Add("ManagementLevelCode", managementLevel.ManagementLevelCode);
                p.Add("Rank", managementLevel.Rank);
                p.Add("IsDelete", managementLevel.IsDelete);
                p.Add("CreateAt", managementLevel.CreateAt);
                p.Add("CreateBy", managementLevel.CreateBy);
                int affectedRows = _managementLevelService.AddManagementLevel(p);
                return affectedRows.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPut]
        [Route("[action]")]
        public string Update(ManagementLevelDTO managementLevel)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("ID", managementLevel.ID);
                p.Add("ManagementLevelName", managementLevel.ManagementLevelName);
                p.Add("ManagementLevelCode", managementLevel.ManagementLevelCode);
                p.Add("Rank", managementLevel.Rank);
                p.Add("IsDelete", managementLevel.IsDelete);
                p.Add("UpdateAt", managementLevel.CreateAt);
                p.Add("UpdateBy", managementLevel.CreateBy);
                int affectedRows = _managementLevelService.UpdateManagementLevel(p);
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
                List<ManagementLevelDTO> data = _managementLevelService.GetByID(ID);
                ResponseModel<List<ManagementLevelDTO>> response = new ResponseModel<List<ManagementLevelDTO>>()
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
                int rs = _managementLevelService.deleteManagementLevel(ID);
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

