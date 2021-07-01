using ISO.API.Models.HumanProfileDiploma;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;
using ISO.API.Models.ProfileHumanPermission;
using Newtonsoft.Json;
using System;

namespace ISO.API.Service.Implement
{
    public class HumanProfileDiplomaService : IHumanProfileDiplomaService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3HumanProfileDiploma_Delete", new { ID = ID });
            return result;
        }

        public List<HumanProfileDiplomaResponse> GetByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileDiplomaResponse> lst = exec.GetByField<HumanProfileDiplomaResponse>("sp_V3HumanProfileDiploma_GetByEmployeeID", param,commandType: CommandType.StoredProcedure);
            return lst;
        }

        public HumanProfileDiplomaDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileDiplomaDTO> lst = exec.GetByField<HumanProfileDiplomaDTO>("sp_V3HumanProfileDiploma_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public HumanProfileDiplomaResponse GetDetailByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileDiplomaResponse> lst = exec.GetByField<HumanProfileDiplomaResponse>("sp_V3HumanProfileDiploma_GetDetailByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<HumanProfileDiplomaExcel> getListDiplomaByEmpID(string lstEmployeeID)
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
            List<HumanProfileDiplomaExcel> result = exec.GetByField<HumanProfileDiplomaExcel>("sp_V3HumanDiploma_GetByEmpID", dynamicParameters, commandType: CommandType.StoredProcedure);
                return result;
        }

        public List<HumanProfileDiplomaResponse> GetPagingByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanProfileDiplomaResponse> lst = exec.GetPaging<HumanProfileDiplomaResponse>(out totalRows, "sp_V3HumanProfileDiploma_GetPagingByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int Insert(HumanProfileDiplomaDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("Faculty", entity.Faculty);
            param.Add("EducationFieldID", entity.EducationFieldID);
            param.Add("StartDate", entity.StartDate);
            param.Add("EndDate", entity.EndDate);
            param.Add("EducationLevelID", entity.EducationLevelID);
            param.Add("EducationTypeID", entity.EducationTypeID);
            param.Add("CertificateLevelID", entity.CertificateLevelID);
            param.Add("EducationOrgID", entity.EducationOrgID);
            param.Add("Condition", entity.Condition);
            param.Add("DateOfGraduation", entity.DateOfGraduation);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileDiploma_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(HumanProfileDiplomaDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("Faculty", entity.Faculty);
            param.Add("EducationFieldID", entity.EducationFieldID);
            param.Add("StartDate", entity.StartDate);
            param.Add("EndDate", entity.EndDate);
            param.Add("EducationLevelID", entity.EducationLevelID);
            param.Add("EducationTypeID", entity.EducationTypeID);
            param.Add("CertificateLevelID", entity.CertificateLevelID);
            param.Add("EducationOrgID", entity.EducationOrgID);
            param.Add("Condition", entity.Condition);
            param.Add("DateOfGraduation", entity.DateOfGraduation);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3HumanProfileDiploma_Update", param);
            return result;
        }

    }
}
