using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.HumanDashboard;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class HumanDashboardService : IHumanDashboardService
    {
        public List<ChartDTO> GetAmountEmployeeByMothOfYear(int year)
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { Year = year };
            List<ChartDTO> lst = exec.GetByField<ChartDTO>("sp_V3HumanDashboard_CountEmployeeByMothOfYear", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int CountEmployeeTotal()
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<EmpployeeTotalDTO> lst = exec.GetByField<EmpployeeTotalDTO>("sp_V3HumanDashboard_CountEmployeeTotal", null, commandType: CommandType.StoredProcedure);
            return lst[0].Total;
        }

        public List<ChartDTO> GetStatisticalByAge()
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<ChartDTO> lst = exec.GetByField<ChartDTO>("sp_V3HumanDashboard_StatisticalByAge", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<ChartDTO> GetStatisticalByContractType()
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<ChartDTO> lst = exec.GetByField<ChartDTO>("sp_V3HumanDashboard_StatisticalByContractType", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<RecruitmentDTO> GetStatisticalByCruitment()
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<RecruitmentDTO> lst = exec.GetByField<RecruitmentDTO>("sp_V3HumanDashboard_StatisticalByCruitment", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<EmployeeByDepartmentDTO> GetStatisticalByDepartment()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<EmployeeByDepartmentDTO> lst = exec.GetByField<EmployeeByDepartmentDTO>("sp_V3HumanDashboard_StatisticalByDepartment", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<ChartDTO> GetStatisticalByEducationLevel()
        {

            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<ChartDTO> lst = exec.GetByField<ChartDTO>("sp_V3HumanDashboard_StatisticalByEducationLevel", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<AlmostExpireEmployeeDTO> GetStatisticalByExpireDate()
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<AlmostExpireEmployeeDTO> lst = exec.GetByField<AlmostExpireEmployeeDTO>("sp_V3HumanDashboard_StatisticalByExpireDate", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<ChartDTO> GetStatisticalByGender()
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<ChartDTO> lst = exec.GetByField<ChartDTO>("sp_V3HumanDashboard_StatisticalByGender", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<NewEmployeeDTO> GetStatisticalByNewEmployee()
        {
            //0k
            ExecuteQuery exec = new ExecuteQuery();
            List<NewEmployeeDTO> lst = exec.GetByField<NewEmployeeDTO>("sp_V3HumanDashboard_StatisticalByNewEmployee", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<RewardDTO> GetStatisticalByReward()
        {
            //ok
            ExecuteQuery exec = new ExecuteQuery();
            List<RewardDTO> lst = exec.GetByField<RewardDTO>("sp_V3HumanDashboard_StatisticalByReward", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<TrainingDTO> GetStatisticalByTraining()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<TrainingDTO> lst = exec.GetByField<TrainingDTO>("sp_V3HumanDashboard_StatisticalByTraining", null, commandType: CommandType.StoredProcedure);
            return lst;
        }
    }
}
