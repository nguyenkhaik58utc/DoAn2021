using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.Ethnic;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class EthnicService : IEthnicService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstEthnic_Delete", new { ID = ID });
            return result;
        }

        public List<EthnicDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
         
            List<EthnicDTO> lst = exec.GetByField<EthnicDTO>("sp_V3MstEthnic_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public EthnicDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<EthnicDTO> lst = exec.GetByField<EthnicDTO>("sp_V3MstEthnic_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(EthnicDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("NationalityID", entity.NationalityID);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstEthnic_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(EthnicDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("ID", entity.ID);
            param.Add("NationalityID", entity.NationalityID);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstEthnic_Update", param);
            return result;
        }
    }
}
