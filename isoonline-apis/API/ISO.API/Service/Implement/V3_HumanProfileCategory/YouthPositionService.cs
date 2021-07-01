using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.YouthPosition;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class YouthPositionService : IYouthPositionService
    {
        public YouthPositionDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<YouthPositionDTO> lst = exec.GetByField<YouthPositionDTO>("sp_V3MstYouthPosition_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<YouthPositionDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<YouthPositionDTO> lst = exec.GetByField<YouthPositionDTO>("sp_V3MstYouthPosition_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int Insert(YouthPositionDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstYouthPosition_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(YouthPositionDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstYouthPosition_Update", param);
            return result;
        }

        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstYouthPosition_Delete", new { ID = ID });
            return result;
        }
    }
}
