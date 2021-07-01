using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanDashboard;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IHumanDashboardService
    {
        /// <summary>
        /// Lấy danh sách biến động nhân sự theo tháng của từng năm
        /// </summary>
        /// <param name="year">năm</param>
        /// <returns></returns>
        List<ChartDTO> GetAmountEmployeeByMothOfYear(int year);

        /// <summary>
        /// Lấy tổng số nhân viên hiện tại
        /// </summary>
        /// <returns></returns>
        int CountEmployeeTotal();

        /// <summary>
        /// Thống kê trình độ học vấn
        /// </summary>
        /// <returns></returns>
        List<ChartDTO> GetStatisticalByEducationLevel();

        /// <summary>
        /// Danh sách khen thưởng nhân sự trong tháng
        /// </summary>
        /// <returns></returns>
        List<RewardDTO> GetStatisticalByReward();

        /// <summary>
        /// Danh sách nhân viên mới
        /// </summary>
        /// <returns></returns>
        List<NewEmployeeDTO> GetStatisticalByNewEmployee();

        /// <summary>
        /// Danh sách nhân viên sắp hết hạn hợp đồng
        /// </summary>
        /// <returns></returns>
        List<AlmostExpireEmployeeDTO> GetStatisticalByExpireDate();

        /// <summary>
        /// Danh sách đợt tuyển dụng sắp tới
        /// </summary>
        /// <returns></returns>
        List<RecruitmentDTO> GetStatisticalByCruitment();


        /// <summary>
        /// Danh sách đợt đào tạo sắp tới
        /// </summary>
        /// <returns></returns>
        List<TrainingDTO> GetStatisticalByTraining();

        /// <summary>
        /// Danh sách thống kê nhân sự theo phòng ban
        /// </summary>
        /// <returns></returns>
        List<EmployeeByDepartmentDTO> GetStatisticalByDepartment();

        /// <summary>
        /// Danh sách thống kê nhân sự theo độ tuổi
        /// </summary>
        /// <returns></returns>
        List<ChartDTO> GetStatisticalByAge();

        /// <summary>
        /// Danh sách thống kê nhân sự theo giới tính
        /// </summary>
        /// <returns></returns>
        List<ChartDTO> GetStatisticalByGender();


        /// <summary>
        /// Danh sách thống kê nhân sự theo loại hợp đồng
        /// </summary>
        /// <returns></returns>
        List<ChartDTO> GetStatisticalByContractType();
    }
}
