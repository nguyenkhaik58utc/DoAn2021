using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.Commune;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class CommuneService : ICommuneService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstCommune_Delete", new { ID = ID });
            return result;
        }

        public List<CommuneDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<CommuneDTO> lst = exec.GetByField<CommuneDTO>("sp_V3MstCommune_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<CommuneDTO> GetPaging(out int totalRows, int pageSize, int pageNumber)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<CommuneDTO> lst = exec.GetPaging<CommuneDTO>(out totalRows, "sp_V3MstCommune_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<CommuneDTO> GetByDistrict(int districtID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DistrictID = districtID };

            List<CommuneDTO> lst = exec.GetByField<CommuneDTO>("sp_V3MstCommune_GetByDistrict", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public CommuneDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<CommuneDTO> lst = exec.GetByField<CommuneDTO>("sp_V3MstCommune_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }



        public int Insert(CommuneDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("DistrictId", entity.DistrictId);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstCommune_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }


        public int Update(CommuneDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("DistrictId", entity.DistrictId);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstCommune_Update", param);
            return result;
        }
    }
}
