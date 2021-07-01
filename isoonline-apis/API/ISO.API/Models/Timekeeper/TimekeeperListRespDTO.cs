using System.Collections.Generic;

namespace ISO.API.Models
{
    public class TimekeeperListRespDTO
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public List<TimekeeperDTO> lstTimekeeper { get; set; }

        public TimekeeperListRespDTO()
        {
            lstTimekeeper = new List<TimekeeperDTO>();
        }
    }
}
