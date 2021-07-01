using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IDepartmentTitleUserService
    {
        int InsertUpdate(DepartmentTitleUserInsUpdModel requestModel);
        List<DepartmentTitleUserDTO> GetByEmployee(int employeeID);
        int DeleteByDepartmentTitleAndUser(int departmentTitleID, int userID, int updatedBy, DateTime updatedAt);
    }
}
