using ISO.API.Models.PoliticalTheory;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.City;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class PoliticalTheoryService : IPoliticalTheoryService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstPoliticalTheory_Delete", new { ID = ID });
            return result;
        }

        public List<PoliticalTheoryDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<PoliticalTheoryDTO> lst = exec.GetByField<PoliticalTheoryDTO>("sp_V3MstPoliticalTheory_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public PoliticalTheoryDTO GetByID(int ID)
        {
         

            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<PoliticalTheoryDTO> lst = exec.GetByField<PoliticalTheoryDTO>("sp_V3MstPoliticalTheory_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(PoliticalTheoryDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstPoliticalTheory_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(PoliticalTheoryDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstPoliticalTheory_Update", param);
            return result;
        }
    }
}
