﻿using ISO.API.Models.DisciplineCategory;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;

namespace ISO.API.Service.Implement
{
    public class DisciplineCategoryService : IDisciplineCategoryService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstDisciplineCategory_Delete", new { ID = ID });
            return result;
        }

        public List<DisciplineCategoryDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<DisciplineCategoryDTO> lst = exec.GetByField<DisciplineCategoryDTO>("sp_V3MstDisciplineCategory_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public DisciplineCategoryDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<DisciplineCategoryDTO> lst = exec.GetByField<DisciplineCategoryDTO>("sp_V3MstDisciplineCategory_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(DisciplineCategoryDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstDisciplineCategory_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(DisciplineCategoryDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstDisciplineCategory_Update", param);
            return result;
        }
    }
}
