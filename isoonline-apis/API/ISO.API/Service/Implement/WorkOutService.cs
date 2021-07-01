using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.DepartmentTitle;
using ISO.API.Models.WorkOut;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class WorkOutService : IWorkOutService
    {
        private readonly IDepartmentTitleService _departmentTitleService;

        public WorkOutService(IDepartmentTitleService departmentTitleService)
        {
            _departmentTitleService = departmentTitleService;
        }

        public List<WorkOutViewModel> GetByMonth(out int totalRows, GetWorkOutByMonthRequest request)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeId", request.EmployeeId, DbType.Int32);
            param.Add("@Month", request.Month, DbType.Int32);
            param.Add("@Year", request.Year, DbType.Int32);
            param.Add("@Status", request.Status, DbType.Int32);
            param.Add("@PageSize", request.PageSize, DbType.Int32);
            param.Add("@PageNumber", request.PageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<WorkOutViewModel> result = exec.GetPaging<WorkOutViewModel>(out totalRows, "sp_V3EmployeeWorkOut_GetByMonth", param, "TotalRows", commandType: CommandType.StoredProcedure);
            foreach (var item in result)
            {
                item.ListDepartmentTitle = _departmentTitleService.GetListByEmployeeID(item.EmployeeId);
            }

            return result;
        }

        public int Insert(InsertWorkOutRequest request)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();

            param.Add("@EmployeeId", request.EmployeeId, DbType.Int32);
            param.Add("@StartTime", request.StartTime, DbType.DateTime);
            param.Add("@EndTime", request.EndTime, DbType.DateTime);
            param.Add("@Address", request.Address, DbType.String);
            param.Add("@Reason", request.Reason, DbType.String);
            param.Add("@ApprovedBy", request.ApprovedBy, DbType.Int32);
            param.Add("@Choice", "insert", DbType.String);

            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3EmployeeWorkOut_CRUD", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(UpdateWorkOutRequest request)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();

            param.Add("@Id", request.Id, DbType.Int32);
            param.Add("@StartTime", request.StartTime, DbType.DateTime);
            param.Add("@EndTime", request.EndTime, DbType.DateTime);
            param.Add("@Address", request.Address, DbType.String);
            param.Add("@Reason", request.Reason, DbType.String);
            param.Add("@ApprovedBy", request.ApprovedBy, DbType.Int32);
            param.Add("@Choice", "update", DbType.String);

            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3EmployeeWorkOut_CRUD", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int CheckExist(CheckExistWorkOutRequest request)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();

            param.Add("@EmployeeId", request.EmployeeId, DbType.Int32);
            param.Add("@StartTime", request.StartTime, DbType.DateTime);
            param.Add("@EndTime", request.EndTime, DbType.DateTime);

            param.Add("@ReturnCount", DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3EmployeeWorkOut_CheckExist", param, "@ReturnCount", commandType: CommandType.StoredProcedure);
            return result;
        }

        public WorkOutDTO GetById(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new
            {
                Id = id
            };

            WorkOutDTO result = exec.GetByField<WorkOutDTO>("sp_V3EmployeeWorkOut_GetById", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public int Delete(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();

            param.Add("@Id", id, DbType.Int32);
            param.Add("@Choice", "delete", DbType.String);

            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3EmployeeWorkOut_CRUD", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}