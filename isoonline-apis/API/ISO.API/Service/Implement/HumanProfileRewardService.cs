using ISO.API.Models.HumanProfileReward;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System;
using ISO.API.Models.ProfileHumanPermission;

namespace ISO.API.Service.Implement
{
    public class HumanProfileRewardService : IHumanProfileRewardService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3HumanProfileReward_Delete", new { ID = ID });
            return result;
        }

        public List<HumanProfileRewardExcel> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileRewardExcel> lst = exec.GetByField<HumanProfileRewardExcel>("sp_V3HumanProfileReward_GetAllByEmployeeID", param ,commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanProfileRewardResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanProfileRewardResponse> lst = exec.GetPaging<HumanProfileRewardResponse>(out totalRows, "sp_V3HumanProfileReward_GetByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public HumanProfileRewardDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileRewardDTO> lst = exec.GetByField<HumanProfileRewardDTO>("sp_V3HumanProfileReward_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public HumanProfileRewardResponse GetDetailByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileRewardResponse> lst = exec.GetByField<HumanProfileRewardResponse>("sp_V3HumanProfileReward_GetDetailByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<HumanProfileRewardExcel> getListRewardByEmpID(string lstEmployeeID)
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
            List<HumanProfileRewardExcel> result = exec.GetByField<HumanProfileRewardExcel>("sp_V3HumanProfileListReward", dynamicParameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Insert(HumanProfileRewardDTO entity)
        {

            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("NumberOfDecision", entity.NumberOfDecision);
            param.Add("DateOfDecision", entity.DateOfDecision);
            param.Add("Reason", entity.Reason);
            param.Add("AwardCategoryID", entity.AwardCategoryID);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileReward_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(HumanProfileRewardDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("NumberOfDecision", entity.NumberOfDecision);
            param.Add("DateOfDecision", entity.DateOfDecision);
            param.Add("Reason", entity.Reason);
            param.Add("AwardCategoryID", entity.AwardCategoryID);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3HumanProfileReward_Update", param);
            return result;
        }
    }
}
