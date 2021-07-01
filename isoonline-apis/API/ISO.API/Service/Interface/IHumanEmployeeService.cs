using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IHumanEmployeeService
    {
        List<HumanEmployeeDTO> GetByDepartment(int departmentID);

        List<HumanEmployeeResponse> GetListByDepartmentID(out int totalRows, HumanEmployeeSearchRequest req);

        List<HumanEmployeeResponse> GetListALL(out int totalRows, int pageSize, int pageNumber);

        List<HumanEmployeeResponse> GetByListEmployeeId(string lstID);

        List<HumanEmployeeResponse> GetByDocument(int documentId);

        HumanEmployeeResponse GetById(int id);
    }
}