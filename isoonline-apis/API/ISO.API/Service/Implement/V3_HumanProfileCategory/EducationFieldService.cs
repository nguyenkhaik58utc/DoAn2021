using ISO.API.Models.EducationField;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;


namespace ISO.API.Service.Implement
{
    public class EducationFieldService : IEducationFieldService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstEducationField_Delete", new { ID = ID });
            return result;
        }

        public List<EducationFieldDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<EducationFieldDTO> lst = exec.GetByField<EducationFieldDTO>("sp_V3MstEducationField_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public EducationFieldDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<EducationFieldDTO> lst = exec.GetByField<EducationFieldDTO>("sp_V3MstEducationField_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(EducationFieldDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstEducationField_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(EducationFieldDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstEducationField_Update", param);
            return result;
        }
    }
}
