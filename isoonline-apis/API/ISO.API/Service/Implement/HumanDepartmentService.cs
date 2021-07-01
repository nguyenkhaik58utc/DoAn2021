using ISO.API.DataAccess;
using ISO.API.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISO.API.Service
{
    public class HumanDepartmentService : IHumanDepartmentService
    {
        public List<HumanDepartmentDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<HumanDepartmentDTO> departmentDTOs = exec.GetAll<HumanDepartmentDTO>("sp_HumanDepartments_GetAll", commandType: CommandType.StoredProcedure);
            return departmentDTOs;
        }

        public HumanDepartmentDTO GetByID(int depID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var obj = new { DepartmentID = depID };
            HumanDepartmentDTO departmentDTOs = exec.GetByField<HumanDepartmentDTO>("sp_HumanDepartments_GetByID", obj, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return departmentDTOs;
        }
    }
}
