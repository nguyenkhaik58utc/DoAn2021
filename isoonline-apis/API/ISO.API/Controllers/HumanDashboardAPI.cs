using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Models.Ages;
using ISO.API.Models.DepartmentTitle;
using ISO.API.Models.HumanDashboard;
using ISO.API.Service;
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
    [SwaggerTag("API quản lý dashboard nhân sự")]
    [RsaAuthorize]
    public class HumanDashboardAPI : ControllerBase
    {
        private readonly IHumanDashboardService _iHumanDashboardService;
        private readonly IDepartmentTitleService _departmentTitleService;

        public HumanDashboardAPI(IHumanDashboardService iHumanDashboardService, IDepartmentTitleService departmentTitleService)
        {
            _iHumanDashboardService = iHumanDashboardService;
            _departmentTitleService = departmentTitleService;
        }

        /// <summary>
        /// Lấy danh sách biến động nhân sự theo tháng của từng năm
        /// </summary>
        /// <param name="year">năm</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAmountEmployeeByMothOfYear(int year)
        {
            try
            {
                var data = _iHumanDashboardService.GetAmountEmployeeByMothOfYear(year);
                var total = _iHumanDashboardService.CountEmployeeTotal();
                ResponseModel<List<ChartDTO>> response = new ResponseModel<List<ChartDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = data,
                    TotalRows= total
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
        /// Thống kê trình độ học vấn
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByEducationLevel()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByEducationLevel();
                ResponseModel<List<ChartDTO>> response = new ResponseModel<List<ChartDTO>>()
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
        /// Danh sách khen thưởng nhân sự trong tháng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByReward()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByReward();
                ResponseModel<List<RewardDTO>> response = new ResponseModel<List<RewardDTO>>()
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
        /// Danh sách nhân viên mới
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByNewEmployee()
        {
            try
            {
                var humanEmployeeList = _iHumanDashboardService.GetStatisticalByNewEmployee();
                foreach (var item in humanEmployeeList)
                {
                    item.ListDepartmentTitle = _departmentTitleService.GetListByEmployeeID(item.ID);
                }


                foreach (var item in humanEmployeeList)
                {
                    for (int i = 0; i < item.ListDepartmentTitle.Count; i++)
                    {
                        item.DepartmentName += item.ListDepartmentTitle[i].TitleName + " -- " + item.ListDepartmentTitle[i].DepartmentName +  "</br>";
                    }
                    //if (item.DepartmentName.Length !=0)
                    //item.DepartmentName.Remove(item.DepartmentName.Length-4, item.DepartmentName.Length-1);
                }
                ResponseModel<List<NewEmployeeDTO>> response = new ResponseModel<List<NewEmployeeDTO>>()
                {
                    MessageCode = "0000",
                    Message = "Success!",
                    Data = humanEmployeeList
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
        /// Danh sách nhân viên sắp hết hạn hợp đồng
        /// </summary>
        /// <param name="year">năm</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByExpireDate()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByExpireDate();
                ResponseModel<List<AlmostExpireEmployeeDTO>> response = new ResponseModel<List<AlmostExpireEmployeeDTO>>()
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
        /// Danh sách đợt tuyển dụng sắp tới
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByCruitment()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByCruitment();
                ResponseModel<List<RecruitmentDTO>> response = new ResponseModel<List<RecruitmentDTO>>()
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
        /// Danh sách đợt đào tạo sắp tới
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByTraining()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByTraining();
                ResponseModel<List<TrainingDTO>> response = new ResponseModel<List<TrainingDTO>>()
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
        /// Danh sách thống kê nhân sự theo phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByDepartment()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByDepartment();
                ResponseModel<List<EmployeeByDepartmentDTO>> response = new ResponseModel<List<EmployeeByDepartmentDTO>>()
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
        /// Danh sách thống kê nhân sự theo độ tuổi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByAge()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByAge();
                ResponseModel<List<ChartDTO>> response = new ResponseModel<List<ChartDTO>>()
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
        /// Danh sách thống kê nhân sự theo giới tính
        /// </summary>
        /// <param name="year">năm</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByGender()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByGender();
                ResponseModel<List<ChartDTO>> response = new ResponseModel<List<ChartDTO>>()
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
        /// Danh sách thống kê nhân sự theo loại hợp đồng
        /// </summary>
        /// <param name="year">năm</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatisticalByContractType()
        {
            try
            {
                var data = _iHumanDashboardService.GetStatisticalByContractType();
                ResponseModel<List<ChartDTO>> response = new ResponseModel<List<ChartDTO>>()
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