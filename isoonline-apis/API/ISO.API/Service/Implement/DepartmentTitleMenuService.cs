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
    public class DepartmentTitleMenuService : IDepartmentTitleMenuService
    {
        public List<DepartmentTitleMenuDTO> GetByDepartmentAndTitle(int departmentID, int titleID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DepartmentID = departmentID, TitleID = titleID };
            List<DepartmentTitleMenuDTO> departmentTitleMenuDTOs = exec.GetByField<DepartmentTitleMenuDTO>("sp_DepartmentTitleMenu_GetByDepartmentAndTitle", param, commandType: CommandType.StoredProcedure);
            return departmentTitleMenuDTOs;
        }

        public List<DepartmentTitleMenuDTO> GetByUser(int userID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { UserID = userID };
            List<DepartmentTitleMenuDTO> departmentTitleMenuDTOs = exec.GetByField<DepartmentTitleMenuDTO>("sp_V3DepartmentTitleMenu_GetByUser", param, commandType: CommandType.StoredProcedure);
            return departmentTitleMenuDTOs;
        }

        public int Insert(DepartmentTitleMenuInsModel departmentTitleMenuDTO)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@CreatedAt", departmentTitleMenuDTO.CreatedAt, DbType.DateTime);
            param.Add("@CreatedBy", departmentTitleMenuDTO.CreatedBy, DbType.Int32);
            param.Add("@DepartmentTitleID", departmentTitleMenuDTO.DepartmentTitleID, DbType.Int32);
            param.Add("@IsDeleted", departmentTitleMenuDTO.IsDeleted, DbType.Boolean);
            param.Add("@MenuID", departmentTitleMenuDTO.MenuID, DbType.Int32);
            param.Add("@ArrMenuID", departmentTitleMenuDTO.StrArrMenuID, DbType.String);
            param.Add("@ReturnID", DbType.Int32, direction: ParameterDirection.Output);
            var rs = exec.ExcuteReturnValue("sp_V3DepartmentTitleMenu_Ins", param, "@ReturnID", commandType: CommandType.StoredProcedure);
            return rs;
        }

        // HungNM
        public List<TreeRollMenuV3DTO> GetAllTreeRollMenuV3()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<TreeRollMenuV3DTO> lstTreeRollMenu = exec.GetAll<TreeRollMenuV3DTO>("sp_GetAllTreeRollMenuV3", commandType: CommandType.StoredProcedure);
            return lstTreeRollMenu;
        }
        public bool UpdateDepartmentTitleMenuV3(CRUDDepartmentTitleMenuV3DTO param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new
            {
                DepartmentID = param.DepartmentID,
                TitleID = param.TitleID,
                lstAdd = param.lstAdd,
                lstDelete = param.lstDelete
            };

            var rs = exec.ExcuteScalar("sp_CRUDDepartmentTitleMenuV3", p);
            return rs > 0;
        }
        public bool CopyMenuTitleRoleV3(CopyMenuTitleRoleV3DTO param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new
            {
                srcDepartmentID = param.srcDepartmentID,
                srcTitleID = param.srcTitleID,
                desDepartmentID = param.desDepartmentID,
                desTitleID = param.desTitleID
            };

            var rs = exec.ExcuteScalar("sp_CopyMenuTitleRoleV3", p);
            return rs > 0;
        }
        // End. HungNM.
    }
}
