using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IDepartmentTitleMenuService
    {
        List<DepartmentTitleMenuDTO> GetByDepartmentAndTitle(int departmentID, int titleID);
        int Insert(DepartmentTitleMenuInsModel departmentTitleDTO);
        List<DepartmentTitleMenuDTO> GetByUser(int userID);

        // HungNM.
        List<TreeRollMenuV3DTO> GetAllTreeRollMenuV3();
        bool UpdateDepartmentTitleMenuV3(CRUDDepartmentTitleMenuV3DTO param);
        bool CopyMenuTitleRoleV3(CopyMenuTitleRoleV3DTO param);
        // End. HungNM.
    }
}
