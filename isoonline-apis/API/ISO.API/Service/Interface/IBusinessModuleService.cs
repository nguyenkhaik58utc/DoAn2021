using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IBusinessModuleService
    {
        List<BusinessModuleDTO> GetPaging(out int totalRows, BusinessModuleReqModel reqModel);
    }
}
