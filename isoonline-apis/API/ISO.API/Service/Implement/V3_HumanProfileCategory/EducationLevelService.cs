using ISO.API.Models.EducationLevel;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.City;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class EducationLevelService : IEducationLevelService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstEducationLevel_Delete", new { ID = ID });
            return result;
        }

        public List<EducationLevelDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<EducationLevelDTO> lst = exec.GetByField<EducationLevelDTO>("sp_V3MstEducationLevel_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public EducationLevelDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<EducationLevelDTO> lst = exec.GetByField<EducationLevelDTO>("sp_V3MstEducationLevel_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(EducationLevelDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstEducationLevel_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(EducationLevelDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstEducationLevel_Update", param);
            return result;
        }
    }
}
