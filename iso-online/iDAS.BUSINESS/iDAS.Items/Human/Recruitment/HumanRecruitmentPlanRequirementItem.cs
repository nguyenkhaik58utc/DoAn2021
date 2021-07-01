using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentPlanRequirementItem
    {
        public int ID { get; set; }
        public int RequiredID { get; set; }
        public int PlanID { get; set; }
        /// <summary>
        /// Tên yêu cầu tuyển dụng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lý do tuyển dụng
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Có được lựa chọn bởi kế hoạch
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        /// Tên chức danh tuyển dụng
        /// </summary>
        public string RoleName { get; set; }
    }
}