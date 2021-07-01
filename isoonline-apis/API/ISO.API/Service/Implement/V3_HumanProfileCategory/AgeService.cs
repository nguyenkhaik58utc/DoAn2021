﻿using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.Ages;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class AgeService : IAgeService
    {

        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstAges_Delete", new { ID = ID });
            return result;
        }

        public List<AgeDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<AgeDTO> lst = exec.GetByField<AgeDTO>("sp_V3MstAges_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public AgeDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<AgeDTO> lst = exec.GetByField<AgeDTO>("sp_V3MstAges_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(AgeDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("FromAge", entity.FromAge);
            param.Add("ToAge", entity.ToAge);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstAges_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(AgeDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("FromAge", entity.FromAge);
            param.Add("ToAge", entity.ToAge);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstAges_Update", param);
            return result;
        }
    }
}
