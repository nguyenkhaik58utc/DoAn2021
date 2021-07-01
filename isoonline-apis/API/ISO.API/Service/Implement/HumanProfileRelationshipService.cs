using ISO.API.Models.HumanProfileRelationship;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;

namespace ISO.API.Service.Implement
{
    public class HumanProfileRelationshipService : IHumanProfileRelationshipService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3HumanProfileRelationship_Delete", new { ID = ID });
            return result;
        }

        public List<HumanProfileRelationshipResponse> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileRelationshipResponse> lst = exec.GetByField<HumanProfileRelationshipResponse>("sp_V3HumanProfileRelationship_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanProfileRelationshipResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanProfileRelationshipResponse> lst = exec.GetPaging<HumanProfileRelationshipResponse>(out totalRows, "sp_V3HumanProfileRelationship_GetByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        //public List<HumanProfileRelationshipDTO> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        //{
        //    ExecuteQuery exec = new ExecuteQuery();
        //    DynamicParameters param = new DynamicParameters();
        //    param.Add("@EmployeeID", employeeID, DbType.Int32);
        //    param.Add("@PageSize", pageSize, DbType.Int32);
        //    param.Add("@PageNumber", pageNumber, DbType.Int32);
        //    param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
        //    List<HumanProfileRelationshipDTO> lst = exec.GetPaging<HumanProfileRelationshipDTO>(out totalRows, "sp_V3HumanProfileRelationship_GetByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
        //    return lst;
        //}

        public HumanProfileRelationshipDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileRelationshipDTO> lst = exec.GetByField<HumanProfileRelationshipDTO>("sp_V3HumanProfileRelationship_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public HumanProfileRelationshipResponse GetDetailByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileRelationshipResponse> lst = exec.GetByField<HumanProfileRelationshipResponse>("sp_V3HumanProfileRelationship_GetDetailByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(HumanProfileRelationshipDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("Age", entity.Age);
            param.Add("IsMale", entity.IsMale);
            param.Add("IsSelf", entity.IsSelf);
            param.Add("RelationshipID", entity.RelationshipID);
            param.Add("Job", entity.Job);
            param.Add("PlaceOfJob", entity.PlaceOfJob);
            param.Add("Phone", entity.Phone);
            param.Add("Address", entity.Address);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileRelationship_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(HumanProfileRelationshipDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("Age", entity.Age);
            param.Add("IsMale", entity.IsMale);
            param.Add("IsSelf", entity.IsSelf);
            param.Add("RelationshipID", entity.RelationshipID);
            param.Add("Job", entity.Job);
            param.Add("PlaceOfJob", entity.PlaceOfJob);
            param.Add("Phone", entity.Phone);
            param.Add("Address", entity.Address);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3HumanProfileRelationship_Update", param);
            return result;
        }

    }
}
