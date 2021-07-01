using ISO.API.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
   public interface IDocumentDepartmentService
    {
        List<DocumentDepartmentDTO> GetListByDocumentID(int documentID);

        bool SavePermission(int documentID,List<DocumentDepartmentDTO> lstDepartment);
    }
}
