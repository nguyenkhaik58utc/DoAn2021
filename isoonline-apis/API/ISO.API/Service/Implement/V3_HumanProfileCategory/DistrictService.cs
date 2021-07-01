﻿using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using ISO.API.Models.District;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class DistrictService : IDistrictService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstDistrict_Delete", new { ID = ID });
            return result;
        }

        public List<DistrictDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<DistrictDTO> lst = exec.GetByField<DistrictDTO>("sp_V3MstDistrict_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }


        public List<DistrictDTO> GetByCity(int cityID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { CityID = cityID };

            List<DistrictDTO> lst = exec.GetByField<DistrictDTO>("sp_V3MstDistrict_GetByCity", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public DistrictDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<DistrictDTO> lst = exec.GetByField<DistrictDTO>("sp_V3MstDistrict_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<DistrictDTO> GetPaging(out int totalRows, int pageSize, int pageNumber)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<DistrictDTO> lst = exec.GetPaging<DistrictDTO>(out totalRows, "sp_V3MstDistrict_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int Insert(DistrictDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CityId", entity.CityId);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstDistrict_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(DistrictDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("CityId", entity.CityId);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstDistrict_Update", param);
            return result;
        }
    }
}
