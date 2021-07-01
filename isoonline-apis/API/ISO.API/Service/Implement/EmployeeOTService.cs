using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISO.API.Service
{
    public class EmployeeOTService : IEmployeeOTService
    {
        public EmployeeOTListResp GetData(int pageIndex, int pageSize)
        {
            EmployeeOTListResp resp = new EmployeeOTListResp();
            DynamicParameters param = new DynamicParameters();
            param.Add("PageIndex", pageIndex, dbType: DbType.Int32);
            param.Add("PageSize", pageSize, dbType: DbType.Int32);
            param.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            ExecuteQuery exec = new ExecuteQuery();
            resp.lstEmployee = exec.GetPaging<EmployeeOT>(out int totalRows, "sp_EmployeeOT_GetData", param, "Total", commandType: CommandType.StoredProcedure);
            resp.total = totalRows;
            resp.pageIndex = pageIndex;
            resp.pageSize = pageSize;
            return resp;
        }

        public EmployeeOT GetByID(int ID)
        {

            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<EmployeeOT> lst = exec.GetByField<EmployeeOT>("sp_EmployeeOT_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(EmployeeOT obj)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new DynamicParameters();
            p.Add("StartTime", obj.StartTime);
            p.Add("EndTime", obj.EndTime);
            p.Add("EmployeeID", obj.EmployeeID);
            p.Add("Reason", obj.Reason);
            p.Add("Status", obj.Status);
            p.Add("ApprovedBy", obj.ApprovedBy);
            p.Add("Level", obj.Level);
            p.Add("CreateBy", obj.UserID);
            p.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var rs = exec.ExcuteReturnValue("sp_EmployeeOT_Insert", p, "ID", commandType: CommandType.StoredProcedure);
            return rs;
        }

        public bool Update(EmployeeOT obj)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new
            {
                ID = obj.ID,
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,
                EmployeeID = obj.EmployeeID,
                Reason = obj.Reason,
                Status = obj.Status,
                ApprovedBy = obj.ApprovedBy,
                Level = obj.Level,
                UpdateBy = obj.UserID
            };

            var rs = exec.ExcuteScalar("sp_EmployeeOT_Update", p);
            return rs > 0;
        }

        public bool Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };
            int rs = exec.ExcuteScalar("sp_EmployeeOT_Delete", param);
            return rs > 0;
        }
    }
}
