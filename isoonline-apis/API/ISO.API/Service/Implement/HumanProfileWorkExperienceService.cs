using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.HumanProfileWorkExperience;
using ISO.API.Models.ProfileHumanPermission;
using ISO.API.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class HumanProfileWorkExperienceService : IHumanProfileWorkExperienceService
    {
        public List<HumanProfileWorkExperienceDTO> GetListByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new { EmployeeID = employeeID };
            List<HumanProfileWorkExperienceDTO> result = exec.GetByField<HumanProfileWorkExperienceDTO>("sp_V3HumanProfileWorkExperience_GetByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public List<HumanProfileWorkExperienceDTO> GetListByListEmployeeID(string lstEmployeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var dynamicParameters = new DynamicParameters();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(Int32));
            List<int> lstID = JsonConvert.DeserializeObject<List<int>>(lstEmployeeID);
            foreach (var item in lstID)
            {
                dt.Rows.Add(item);
            }
            dynamicParameters.Add("@lstID", dt.AsTableValuedParameter("[dbo].[EmployeeIDList]"));
            List<HumanProfileWorkExperienceDTO> result = exec.GetByField<HumanProfileWorkExperienceDTO>("sp_V3HumanProfileWorkExperience_GetByListEmployeeID", dynamicParameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
