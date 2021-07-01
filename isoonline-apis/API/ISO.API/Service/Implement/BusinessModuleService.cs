using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class BusinessModuleService : IBusinessModuleService
    {
        public List<BusinessModuleDTO> GetPaging(out int totalRows, BusinessModuleReqModel reqModel)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", reqModel.Code, DbType.String);
            param.Add("@Name", reqModel.Name, DbType.String);
            param.Add("@IsActive", reqModel.IsActive, DbType.Boolean);
            param.Add("@IsDelete", reqModel.IsDelete, DbType.Boolean);
            param.Add("@IsShow", reqModel.IsShow, DbType.Boolean);
            param.Add("@SystemCode", reqModel.SystemCode, DbType.String);
            param.Add("@PageSize", reqModel.PageSize, DbType.Int32);
            param.Add("@PageNumber", reqModel.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<BusinessModuleDTO> businessModules = exec.GetPaging<BusinessModuleDTO>(out totalRows, "sp_BusinessModule_GetPaging", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return businessModules;
        }
    }
}
