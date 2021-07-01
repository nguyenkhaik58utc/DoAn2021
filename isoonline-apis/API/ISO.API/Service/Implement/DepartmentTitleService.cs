using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using ISO.API.Models.DepartmentTitle;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISO.API.Service
{
    public class DepartmentTitleService : IDepartmentTitleService
    {
        public List<DepartmentTitleDTO> GetByDepartment(int departmentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DepartmentID = departmentID };
            List<DepartmentTitleDTO> departmentTitleDTOs = exec.GetByField<DepartmentTitleDTO>("sp_DepartmentTitle_GetByDepartment", param, commandType: CommandType.StoredProcedure);
            return departmentTitleDTOs;
        }

        public List<DepartmentTitleDTO> GetByTitle(int titleID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { TitleID = titleID };
            List<DepartmentTitleDTO> lstReturn = exec.GetByField<DepartmentTitleDTO>("sp_DepartmentTitle_GetByTitle", param, commandType: CommandType.StoredProcedure).ToList();
            return lstReturn;
        }

        public int Insert(DepartmentTitleDTO departmentTitle)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new DynamicParameters();
            p.Add("DepartmentID", departmentTitle.DepartmentID);
            p.Add("TitleID", departmentTitle.TitleID);
            if (string.IsNullOrWhiteSpace(departmentTitle.Note))
                p.Add("Note", departmentTitle.Note);
            if (departmentTitle.ReportToID > 0)
                p.Add("ReportToID", departmentTitle.ReportToID);
            p.Add("IsActive", departmentTitle.IsActive);
            p.Add("IsDelete", departmentTitle.IsDelete);
            p.Add("CreateBy", departmentTitle.UserID);
            p.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var rs = exec.ExcuteReturnValue("sp_DepartmentTitle_Insert", p, "ID", commandType: CommandType.StoredProcedure);
            return rs;
        }

        public bool Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };
            int rs = exec.ExcuteScalar("sp_DepartmentTitle_Delete", param);
            return rs > 0;
        }

        public List<DepartmentTitleGetByDepEmpModel> GetByDepAndEmp(int depID, int empID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DepartmentID = depID, EmployeeID = empID };
            List<DepartmentTitleGetByDepEmpModel> departmentTitleGetByDepEmpModels = exec.GetByField<DepartmentTitleGetByDepEmpModel>("sp_V3OrgDepartmentTitles_GetByDepAndEmp", param, commandType: CommandType.StoredProcedure);
            return departmentTitleGetByDepEmpModels;
        }

        public List<DepartmentTitleResponse> GetListByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { HumanEmployeeID = employeeID };
            List<DepartmentTitleResponse> departmentTitleDTOs = exec.GetByField<DepartmentTitleResponse>("sp_V3DepartmentTitle_GetListByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return departmentTitleDTOs;
        }
    }
}
