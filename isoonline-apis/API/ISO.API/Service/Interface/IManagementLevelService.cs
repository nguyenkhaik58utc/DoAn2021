using ISO.API.Models.ManagementLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IManagementLevelService
    {
        List<ManagementLevelDTO> GetAll();
        int AddManagementLevel(object param);

        int deleteManagementLevel(int id);

        int UpdateManagementLevel(object param);
        List<ManagementLevelDTO> GetByID(int id);
    }
}
