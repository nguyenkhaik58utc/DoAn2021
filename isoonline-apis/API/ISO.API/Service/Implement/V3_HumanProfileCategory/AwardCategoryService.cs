using ISO.API.Models.AwardCategory;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;

namespace ISO.API.Service.Implement
{
    public class AwardCategoryService : IAwardCategoryService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstAwardCategory_Delete", new { ID = ID });
            return result;
        }

        public List<AwardCategoryDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<AwardCategoryDTO> lst = exec.GetByField<AwardCategoryDTO>("sp_V3MstAwardCategory_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public AwardCategoryDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<AwardCategoryDTO> lst = exec.GetByField<AwardCategoryDTO>("sp_V3MstAwardCategory_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(AwardCategoryDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstAwardCategory_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(AwardCategoryDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstAwardCategory_Update", param);
            return result;
        }
    }
}
