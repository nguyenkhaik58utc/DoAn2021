using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.ShiftHistory;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class ShiftHistoryService : IShiftHistoryService
    {
        public ShiftHistoryDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<ShiftHistoryDTO> listShiftHistory = exec.GetByField<ShiftHistoryDTO>("sp_V3ShiftHistory_GetByID", param, commandType: CommandType.StoredProcedure);
            return listShiftHistory[0];
        }


        public int Insert(ShiftHistoryReqModel entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("UserID", entity.UserID);
            param.Add("StartTime", entity.ShiftTime);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3ShiftHistory_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public bool Update(ShiftHistoryReqModel entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new
            {
                ID = entity.ID,
                EndTime = entity.ShiftTime,
            };

            var result = exec.ExcuteScalar("sp_V3ShiftHistory_Update", param);
            return result > 0;
        }

        public ShiftHistoryDTO GetByUserID(int UserID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { UserID = UserID };

            List<ShiftHistoryDTO> listShiftHistory = exec.GetByField<ShiftHistoryDTO>("sp_V3ShiftHistory_GetByUserID", param, commandType: CommandType.StoredProcedure);
            return listShiftHistory[0];
        }

        public bool HandOverShift(ShiftHistoryReqModel entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param1 = new
            {
                ID = entity.ID,
                EndTime = entity.ShiftTime,
            };

            var resultUpdate = exec.ExcuteScalar("sp_V3ShiftHistory_Update", param1);

            var param2 = new DynamicParameters();
            param2.Add("UserID", entity.ToUserID);
            param2.Add("StartTime", entity.ShiftTime);
            param2.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var resultInsert = exec.ExcuteReturnValue("sp_V3ShiftHistory_Insert", param2, "ID", commandType: CommandType.StoredProcedure);

            if (resultUpdate > 0 && resultInsert > 0)
            {
                return true;
            }
            return false;
        }

        public string GetByDepartmentID(int DepartmentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DepartmentID = DepartmentID };

            List<string> listShiftHistory = exec.GetByField<string>("sp_V3ShiftHistory_GetByDepartmentID", param, commandType: CommandType.StoredProcedure);
            
            return string.Join<string>(", ", listShiftHistory);
        }

        public int CountByUserID(int userID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { UserID = userID };
            List<int> result = exec.GetByField<int>("sp_Problem_GetTotalByUser", param, commandType: CommandType.StoredProcedure);
            if (result == null)
                return 0;

            return result.Count;
        }
    }
}
