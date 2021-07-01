using System.Collections.Generic;

namespace ISO.API.Models
{
    public class ProblemEventReport_ByDepartment
    {
        public List<ProblemEventReport_ByDepartmentTypeDTO> problemEventReport_ByDepartmentTypeDTOs { get; set; }
        public List<ProblemEventReport_ByDepartmentCriticalDTO> problemEventReport_ByDepartmentCriticalDTOs { get; set; }
    }
}
