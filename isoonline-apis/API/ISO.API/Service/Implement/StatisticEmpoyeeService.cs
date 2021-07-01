using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using System.Collections.Generic;
using System.Data;

namespace ISO.API.Service
{
    public class StatisticEmpoyeeService : IStatisticEmpoyeeService
    {
        public List<EmployeeTotalTemp> GetEmployeeTotal()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<EmployeeTotalTemp> lst = exec.GetAll<EmployeeTotalTemp>("sp_Statistic_Employee_Total", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanEmployeeResponse> GetListEmp(out int totalRows, int pageSize, int pageNumber, int departmentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@DepartmentID", departmentID, DbType.Int32);
            param.Add("@PageIndex", pageNumber, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@Total", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanEmployeeResponse> humanEmployeeDTOs = exec.GetPaging<HumanEmployeeResponse>(out totalRows, "[sp_Statistic_Employee_Detail]", param, "Total", commandType: CommandType.StoredProcedure);
            return humanEmployeeDTOs;
        }

        public List<EmployeeTitleTotal> GetEmployeeTitleTotal()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<EmployeeTitleTotal> lst = exec.GetAll<EmployeeTitleTotal>("sp_Statistic_Title_Total", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanEmployeeResponse> GetListEmpByTitle(out int totalRows, int pageSize, int pageNumber, int titleID, int depID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@TitleID", titleID, DbType.Int32);
            param.Add("@DepartmentID", depID, DbType.Int32);
            param.Add("@PageIndex", pageNumber, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@Total", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanEmployeeResponse> humanEmployeeDTOs = exec.GetPaging<HumanEmployeeResponse>(out totalRows, "[sp_Statistic_EmployeeTitle_Detail]", param, "Total", commandType: CommandType.StoredProcedure);
            return humanEmployeeDTOs;
        }

    }
}
