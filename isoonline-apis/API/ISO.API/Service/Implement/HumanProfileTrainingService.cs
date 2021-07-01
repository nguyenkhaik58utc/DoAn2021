using ISO.API.Models.HumanProfileTraining;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;
using System;
using Newtonsoft.Json;
using ISO.API.Models.ProfileHumanPermission;

namespace ISO.API.Service.Implement
{
    public class HumanProfileTrainingService : IHumanProfileTrainingService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3HumanProfileTraining_Delete", new { ID = ID });
            return result;
        }

        public List<HumanProfileTrainingResponse> GetByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileTrainingResponse> lst = exec.GetByField<HumanProfileTrainingResponse>("sp_V3HumanProfileTraining_GetByEmployeeID", param,  commandType: CommandType.StoredProcedure);
            return lst;
        }

        public HumanProfileTrainingDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileTrainingDTO> lst = exec.GetByField<HumanProfileTrainingDTO>("sp_V3HumanProfileTraining_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public HumanProfileTrainingResponse GetDetailByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileTrainingResponse> lst = exec.GetByField<HumanProfileTrainingResponse>("sp_V3HumanProfileTraining_GetDetailByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<HumanProfileEducationTrainingExcel> getListTrainingByEmpID(string lstEmployeeID)
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
            List<HumanProfileEducationTrainingExcel> result = exec.GetByField<HumanProfileEducationTrainingExcel>("sp_V3HumanEducationTrainning_GetByEmpID", dynamicParameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public List<HumanProfileTrainingResponse> GetPagingByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanProfileTrainingResponse> lst = exec.GetPaging<HumanProfileTrainingResponse>(out totalRows, "sp_V3HumanProfileTraining_GetPagingByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int Insert(HumanProfileTrainingDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("StartDate", entity.StartDate);
            param.Add("EndDate", entity.EndDate);
            param.Add("EducationTypeID", entity.EducationTypeID);
            param.Add("Content", entity.Content);
            param.Add("Certificate", entity.Certificate);
            param.Add("EducationResultID", entity.EducationResultID);
            param.Add("Reviews", entity.Reviews);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileTraining_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(HumanProfileTrainingDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("StartDate", entity.StartDate);
            param.Add("EndDate", entity.EndDate);
            param.Add("EducationTypeID", entity.EducationTypeID);
            param.Add("Content", entity.Content);
            param.Add("Certificate", entity.Certificate);
            param.Add("EducationResultID", entity.EducationResultID);
            param.Add("Reviews", entity.Reviews);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3HumanProfileTraining_Update", param);
            return result;
        }
    }
}
