using ISO.API.Models.PositionParty;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class PositionPartyService : IPositionPartyService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstPositionParty_Delete", new { ID = ID });
            return result;
        }

        public PositionPartyDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<PositionPartyDTO> lst = exec.GetByField<PositionPartyDTO>("sp_V3MstPositionParty_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<PositionPartyDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<PositionPartyDTO> lst = exec.GetByField<PositionPartyDTO>("sp_V3MstPositionParty_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int Insert(PositionPartyDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstPositionParty_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(PositionPartyDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstPositionParty_Update", param);
            return result;
        }
    }
}
