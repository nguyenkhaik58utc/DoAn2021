using ISO.API.Models.HumanProfileWorkExperience;
using ISO.API.Models.ProfileHumanPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IHumanProfileWorkExperienceService
    {
        List<HumanProfileWorkExperienceDTO> GetListByListEmployeeID(string lstEmployeeID);
        List<HumanProfileWorkExperienceDTO> GetListByEmployeeID(int employeeID);
    }
}
