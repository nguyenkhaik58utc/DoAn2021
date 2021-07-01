using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class TitleListRespDTO
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public List<TitleDTO> lstTitle { get; set; }

        public TitleListRespDTO()
        {
            lstTitle = new List<TitleDTO>();
        }
    }
}
