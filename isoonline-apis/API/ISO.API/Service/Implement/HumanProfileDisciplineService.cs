using ISO.API.Models.HumanProfileDiscipline;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;


namespace ISO.API.Service.Implement
{
    public class HumanProfileDisciplineService : IHumanProfileDisciplineService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3HumanProfileDiscipline_Delete", new { ID = ID });
            return result;
        }

        public List<HumanProfileDisciplineResponse> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileDisciplineResponse> lst = exec.GetByField<HumanProfileDisciplineResponse>("sp_V3HumanProfileDiscipline_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanProfileDisciplineResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanProfileDisciplineResponse> lst = exec.GetPaging<HumanProfileDisciplineResponse>(out totalRows, "sp_V3HumanProfileDiscipline_GetByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public HumanProfileDisciplineDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileDisciplineDTO> lst = exec.GetByField<HumanProfileDisciplineDTO>("sp_V3HumanProfileDiscipline_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public HumanProfileDisciplineResponse GetDetailByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileDisciplineResponse> lst = exec.GetByField<HumanProfileDisciplineResponse>("sp_V3HumanProfileDiscipline_GetDetailByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(HumanProfileDisciplineDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("NumberOfDecision", entity.NumberOfDecision);
            param.Add("DateOfDecision", entity.DateOfDecision);
            param.Add("Reason", entity.Reason);
            param.Add("DisciplineCategoryID", entity.DisciplineCategoryID);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileDiscipline_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(HumanProfileDisciplineDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("NumberOfDecision", entity.NumberOfDecision);
            param.Add("DateOfDecision", entity.DateOfDecision);
            param.Add("Reason", entity.Reason);
            param.Add("DisciplineCategoryID", entity.DisciplineCategoryID);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3HumanProfileDiscipline_Update", param);
            return result;
        }
    }
}
