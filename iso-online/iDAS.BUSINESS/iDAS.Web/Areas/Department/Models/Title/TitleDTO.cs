using System;
using System.Collections.Generic;

namespace iDAS.Web.Areas.Department.Models
{
    public class TitleDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public bool IsActive { get; set; }

        public int UserID { get; set; }
    }
}
