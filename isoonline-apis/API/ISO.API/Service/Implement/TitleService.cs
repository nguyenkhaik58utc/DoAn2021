using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISO.API.Service
{
    public class TitleService : ITitleService
    {
        public TitleListRespDTO GetData(int pageIndex, int pageSize)
        {
            TitleListRespDTO resp = new TitleListRespDTO();
            DynamicParameters param = new DynamicParameters();
            param.Add("PageIndex", pageIndex, dbType: DbType.Int32);
            param.Add("PageSize", pageSize, dbType: DbType.Int32);
            param.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            ExecuteQuery exec = new ExecuteQuery();
            resp.lstTitle = exec.GetPaging<TitleDTO>(out int totalRows, "sp_Title_GetData", param, "Total", commandType: CommandType.StoredProcedure);
            resp.total = totalRows;
            resp.pageIndex = pageIndex;
            resp.pageSize = pageSize;
            return resp;
        }

        public TitleDTO GetByID(int ID)
        {

            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<TitleDTO> lstTitle = exec.GetByField<TitleDTO>("sp_Title_GetByID", param, commandType: CommandType.StoredProcedure);
            return lstTitle[0];
        }

        public int Insert(TitleDTO title)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new DynamicParameters();
            p.Add("Code", title.Code);
            p.Add("Name", title.Name);
            p.Add("PositionId", title.PositionId);
            p.Add("IsActive", title.IsActive);
            p.Add("CreateBy", title.UserID);
            p.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var rs = exec.ExcuteReturnValue("sp_Title_Insert", p, "ID", commandType: CommandType.StoredProcedure);
            return rs;
        }

        public bool Update(TitleDTO title)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new
            {
                ID = title.ID,
                Code = title.Code,
                Name = title.Name,
                PositionId = title.PositionId,
                IsActive = title.IsActive,
                UpdateBy = title.UserID
            };

            var rs = exec.ExcuteScalar("sp_Title_Update", p);
            return rs > 0;
        }

        public bool Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };
            int rs = exec.ExcuteScalar("sp_Title_Delete", param);
            return rs > 0;
        }

        public List<PositionCboDTO> GetComboPosition()
        {
            ExecuteQuery exec = new ExecuteQuery();
            return exec.GetAll<PositionCboDTO>("sp_Position_GetAll"); ;
        }

        public List<TitleDTO> GetNotAsDepartment(int departmentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DepartmentID = departmentID };
            return exec.GetByField<TitleDTO>("sp_Title_GetNotAsDepartment", param, commandType: CommandType.StoredProcedure).ToList();
        }
    }
}
