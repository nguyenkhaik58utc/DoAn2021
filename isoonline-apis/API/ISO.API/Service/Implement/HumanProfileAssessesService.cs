using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.HumanProfileAssesses;
using ISO.API.Models.HumanProfileInsurances;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class HumanProfileAssessesService : IHumanProfileAssessesService
    {
        public List<HumanProfileAssessesDTO> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileAssessesDTO> lst = exec.GetByField<HumanProfileAssessesDTO>("sp_V3HumanProfileAssesses_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }
    }
}
