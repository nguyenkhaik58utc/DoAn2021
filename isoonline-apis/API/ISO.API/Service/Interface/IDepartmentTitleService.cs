using ISO.API.Models;
using ISO.API.Models.DepartmentTitle;
using System.Collections.Generic;

namespace ISO.API.Service
{
    public interface IDepartmentTitleService
    {
        List<DepartmentTitleDTO> GetByDepartment(int departmentID);
        List<DepartmentTitleDTO> GetByTitle(int titleID);
        int Insert(DepartmentTitleDTO departmentTitle);
        bool Delete(int ID);
        List<DepartmentTitleGetByDepEmpModel> GetByDepAndEmp(int depID, int empID);
        /// <summary>
        /// Lấy danh sách chức danh theo EmployeeID
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        List<DepartmentTitleResponse> GetListByEmployeeID(int employeeID);
        
    }
}
