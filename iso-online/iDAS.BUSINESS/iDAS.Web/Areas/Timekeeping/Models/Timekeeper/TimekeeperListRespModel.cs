using System.Collections.Generic;

namespace iDAS.Web.Areas.Timekeeping.Models
{
    public class TimekeeperListRespModel
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public List<TimekeeperDTO> lstTimekeeper { get; set; }

        public TimekeeperListRespModel()
        {
            lstTimekeeper = new List<TimekeeperDTO>();
        }
    }
}
