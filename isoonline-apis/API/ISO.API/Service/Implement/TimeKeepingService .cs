using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.DepartmentTitle;
using ISO.API.Models.TimeKeeping;
using ISO.API.Models.WorkOut;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class TimeKeepingService : ITimeKeepingService
    {
        private readonly IHumanEmployeeService _humanEmployeeService;

        public TimeKeepingService(IHumanEmployeeService humanEmployeeService)
        {
            _humanEmployeeService = humanEmployeeService;
        }

        public List<TimeKeepingOfEmployeeViewModel> GetByMonth(out int totalRows, GetTimeKeepingByMonthRequest request)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeId", request.EmployeeId, DbType.Int32);
            param.Add("@Month", request.Month, DbType.Int32);
            param.Add("@Year", request.Year, DbType.Int32);
            List<TimeKeepingViewModel> result = exec.GetByField<TimeKeepingViewModel>("[sp_V3Timekeeping_GetAllByMonth]", param, commandType: CommandType.StoredProcedure);
            var query = result.GroupBy(test => test.EmployeeID)
                   .Select(grp => grp.First())
                   .ToList();
            List<TimeKeepingOfEmployeeViewModel> lst = new List<TimeKeepingOfEmployeeViewModel>();
            foreach (var item1 in query)
            {
                TimeKeepingOfEmployeeViewModel item = new TimeKeepingOfEmployeeViewModel();
                item.Name = item1.Name;
                item.Code = item1.Code;
                var query2 = result.Where(x => x.EmployeeID == item1.EmployeeID).ToList();
                item.ListTimeKeeping = query2;
                lst.Add(item);
            }
            totalRows = lst.Count;
            return lst;
        }

        public List<TimeKeepingExplanationViewModel> GetExplanationByMonth(out int totalRows, GetTimeKeepingExplanationByMonthRequest request)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Status", request.Status, DbType.Int32);
            param.Add("@Month", request.Month, DbType.Int32);
            param.Add("@Year", request.Year, DbType.Int32);
            List<TimeKeepingExplanationViewModel> result = exec.GetByField<TimeKeepingExplanationViewModel>("sp_V3TimekeepingExplanation_GetByMonth", param, commandType: CommandType.StoredProcedure);
            totalRows = result.Count;
            return result;
        }

        public int DeleteExplanation(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();

            param.Add("@Id", id, DbType.Int32);
            param.Add("@Choice", "DeleteExplanation", DbType.String);

            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3TimeKeepingExplanation_CRUD", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}