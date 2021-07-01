using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.City;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;

namespace ISO.API.Service.Implement
{
    public class CityService : ICityService
    {
        public CityDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<CityDTO> lst = exec.GetByField<CityDTO>("sp_V3MstCity_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<CityDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<CityDTO> lst = exec.GetByField<CityDTO>("sp_V3MstCity_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<CityDTO> GetPaging(out int totalRows, int pageSize, int pageNumber)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<CityDTO> lst = exec.GetPaging<CityDTO>(out totalRows, "sp_V3MstCity_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int Insert(CityDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstCity_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(CityDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstCity_Update", param);
            return result;
        }

        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstCity_Delete", new { ID = ID });
            return result;
        }
    }
}
