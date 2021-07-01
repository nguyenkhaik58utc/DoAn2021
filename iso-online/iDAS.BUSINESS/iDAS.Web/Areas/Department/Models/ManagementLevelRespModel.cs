using iDAS.Items.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department
{
    public class ManagementLevelRespModel
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public List<ManagementLevelItem> lstManagementItem { get; set; }
        public ManagementLevelItem managementLevel { get; set; }
    }
}