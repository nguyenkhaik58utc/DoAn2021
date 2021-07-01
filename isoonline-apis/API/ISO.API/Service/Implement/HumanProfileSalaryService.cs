using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.HumanProfileSalary;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class HumanProfileSalaryService : IHumanProfileSalaryService
    {
        public List<HumanProfileSalaryDTO> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileSalaryDTO> lst = exec.GetByField<HumanProfileSalaryDTO>("sp_V3HumanProfileSalary_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }
    }
}
