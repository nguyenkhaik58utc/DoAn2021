using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDAS.Web.Areas.Department.Models
{
    public class TitleListRespModel
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public List<TitleDTO> lstTitle { get; set; }

        public TitleListRespModel()
        {
            lstTitle = new List<TitleDTO>();
        }
    }
}
