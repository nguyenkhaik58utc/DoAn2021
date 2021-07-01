using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iDAS.Items;
using iDAS.Items.Position;

namespace iDAS.Web.Areas.Position.Models
{
    public class PositiomRespModel
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public List<PositionItem> lstPosition { get; set; }
        public PositionItem position { get; set; }
    }
}