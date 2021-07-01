using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using System.Collections.Generic;

namespace ISO.API.Service
{
    public interface IStatisticEmpoyeeService
    {
        List<EmployeeTotalTemp> GetEmployeeTotal();
        List<HumanEmployeeResponse> GetListEmp(out int totalRows, int pageSize, int pageNumber, int departmentID);
        List<EmployeeTitleTotal> GetEmployeeTitleTotal();
        List<HumanEmployeeResponse> GetListEmpByTitle(out int totalRows, int pageSize, int pageNumber, int titleID, int depID);
    }
}
