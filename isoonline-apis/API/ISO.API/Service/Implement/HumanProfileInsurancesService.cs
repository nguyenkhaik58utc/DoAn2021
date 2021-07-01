using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.HumanProfileInsurances;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class HumanProfileInsurancesService : IHumanProfileInsuranceService
    {
        public List<HumanProfileInsurancesDTO> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileInsurancesDTO> lst = exec.GetByField<HumanProfileInsurancesDTO>("sp_V3HumanProfileInsurances_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }
    }
}
