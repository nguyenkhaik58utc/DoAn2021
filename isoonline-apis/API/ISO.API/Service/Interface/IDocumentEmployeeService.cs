using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IDocumentEmployeeService
    {
        List<HumanEmployeeResponse> GetListByDocumentID(int documentID);

        bool SavePermission(int documentID, List<DocumentEmployeeSaveDTO> lstEmployee);
    }
}
