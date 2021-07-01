using System.Collections.Generic;

namespace ISO.API.Models
{
    public class ProblemEventReport_ByEventRequestDep
    {
        public List<ProblemEventReport_ByEventRequestDepTypeDTO> problemEventReport_ByEventRequestDepTypeDTOs { get; set; }
        public List<ProblemEventReport_ByEventRequestDepCriticalDTO> problemEventReport_ByEventRequestDepCriticalDTOs { get; set; }
    }
}
