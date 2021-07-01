using iDAS.Items;
using System;

namespace iDAS.Web.Areas.Human.Models
{
    public partial class ShiftHistoryDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime ShiftTime { get; set; }
        public int ProblemEventTotal { get; set; }
        public int ToUserID { get; set; }

        public HumanEmployeeViewItem UserTo { get; set; }

        public ShiftHistoryDTO()
        {
            UserTo = new HumanEmployeeViewItem();
        }

    }
}
