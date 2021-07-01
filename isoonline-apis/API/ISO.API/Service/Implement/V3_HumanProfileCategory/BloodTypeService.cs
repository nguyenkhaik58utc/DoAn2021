using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.BloodType;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class BloodTypeService : IBloodTypeService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstBloodType_Delete", new { ID = ID });
            return result;
        }

        public List<BloodTypeDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<BloodTypeDTO> lst = exec.GetByField<BloodTypeDTO>("sp_V3MstBloodType_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public BloodTypeDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<BloodTypeDTO> lst = exec.GetByField<BloodTypeDTO>("sp_V3MstBloodType_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(BloodTypeDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstBloodType_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(BloodTypeDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstBloodType_Update", param);
            return result;
        }
    }
}
