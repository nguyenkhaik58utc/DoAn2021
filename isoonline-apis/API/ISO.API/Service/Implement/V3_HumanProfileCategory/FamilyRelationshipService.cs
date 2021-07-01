using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.FamilyRelationship;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;

namespace ISO.API.Service.Implement
{
    public class FamilyRelationshipService : IFamilyRelationshipService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstFamilyRelationship_Delete", new { ID = ID });
            return result;
        }

        public List<FamilyRelationshipDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<FamilyRelationshipDTO> lst = exec.GetByField<FamilyRelationshipDTO>("sp_V3MstFamilyRelationship_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public FamilyRelationshipDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<FamilyRelationshipDTO> lst = exec.GetByField<FamilyRelationshipDTO>("sp_V3MstFamilyRelationship_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(FamilyRelationshipDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstFamilyRelationship_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(FamilyRelationshipDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstFamilyRelationship_Update", param);
            return result;
        }
    }
}
