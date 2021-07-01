using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class DepartmentTitleUserService : IDepartmentTitleUserService
    {
        public int DeleteByDepartmentTitleAndUser(int departmentTitleID, int userID, int updatedBy, DateTime updatedAt)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DepartmentTitleID = departmentTitleID, EmployeeID = userID, UpdatedBy = updatedBy, UpdatedAt = updatedAt };
            int rs = exec.ExcuteScalar("sp_V3DepartmentTitleUser_DeleteByDepartmentTitleAndUser", param);
            return rs;
        }

        public List<DepartmentTitleUserDTO> GetByEmployee(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { EmployeeID = employeeID };
            List<DepartmentTitleUserDTO> departmentTitleUserDTOs = exec.GetByField<DepartmentTitleUserDTO>("sp_V3DepartmentTitleUser_GetByEmployee", param, commandType: CommandType.StoredProcedure);
            return departmentTitleUserDTOs;
        }

        public int InsertUpdate(DepartmentTitleUserInsUpdModel requestModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", requestModel.ID, DbType.Int32);
            param.Add("@DepartmentTitleID", requestModel.DepartmentTitleID, DbType.Int32);
            param.Add("@HumanEmployeeID", requestModel.HumanEmployeeID, DbType.Int32);
            param.Add("@CreatedAt", requestModel.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", requestModel.CreatedBy, DbType.Int32);
            param.Add("@IsDelete", requestModel.IsDelete, DbType.Boolean);
            param.Add("@UpdatedAt", requestModel.UpdatedAt, DbType.DateTime);
            param.Add("@UpdatedBy", requestModel.UpdatedBy, DbType.Int32);
            param.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3DepartmentTitleUser_InsUpd", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }
    }
}
