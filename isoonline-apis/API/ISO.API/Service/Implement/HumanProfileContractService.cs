using ISO.API.Models.HumanProfileContract;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;

namespace ISO.API.Service.Implement
{
    public class HumanProfileContractService : IHumanProfileContractService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3HumanProfileContract_Delete", new { ID = ID });
            return result;
        }

        public List<HumanProfileContractResponse> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileContractResponse> lst = exec.GetByField<HumanProfileContractResponse>("sp_V3HumanProfileContract_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanProfileContractResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanProfileContractResponse> lst = exec.GetPaging<HumanProfileContractResponse>(out totalRows, "sp_V3HumanProfileContract_GetByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public HumanProfileContractDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileContractDTO> lst = exec.GetByField<HumanProfileContractDTO>("sp_V3HumanProfileContract_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public HumanProfileContractResponse GetDetailByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileContractResponse> lst = exec.GetByField<HumanProfileContractResponse>("sp_V3HumanProfileContract_GetDetailByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(HumanProfileContractDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("NumberOfContracts", entity.NumberOfContracts);
            param.Add("StartDate", entity.StartDate);
            param.Add("EndDate", entity.EndDate);
            param.Add("ContractTypeID", entity.ContractTypeID);
            param.Add("ContractStatusID", entity.ContractStatusID);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileContract_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(HumanProfileContractDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("NumberOfContracts", entity.NumberOfContracts);
            param.Add("StartDate", entity.StartDate);
            param.Add("EndDate", entity.EndDate);
            param.Add("ContractTypeID", entity.ContractTypeID);
            param.Add("ContractStatusID", entity.ContractStatusID);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3HumanProfileContract_Update", param);
            return result;
        }
    }
}
