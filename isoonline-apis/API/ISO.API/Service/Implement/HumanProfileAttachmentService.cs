using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.HumanProfileAttachment;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class HumanProfileAttachmentService : IHumanProfileAttachmentService
    {
        public List<HumanProfileAttachmentDTO> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileAttachmentDTO> lst = exec.GetByField<HumanProfileAttachmentDTO>("sp_V3HumanProfileAttachments_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }
    }
}
