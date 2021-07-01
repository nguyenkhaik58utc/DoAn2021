using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.HumanProfileWorkTrial;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class HumanProfileWorkTrialService : IHumanProfileWorkTrialService
    {
        public List<HumanProfileWorkTrialDTO> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileWorkTrialDTO> lst = exec.GetByField<HumanProfileWorkTrialDTO>("sp_V3HumanProfileWorkTrial_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }
    }
}
