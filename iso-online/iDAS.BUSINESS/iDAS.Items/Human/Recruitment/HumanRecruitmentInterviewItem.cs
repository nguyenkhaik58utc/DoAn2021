using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentInterviewItem
    {

        public int? ID { get; set; }
        public int PorfileInterviewID { get; set; }
        /// <summary>
        /// Tên của đợt phỏng vấn
        /// </summary>
        /// 
        public int PlanInterviewID { get; set; }
        public string PlanInterviewName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public String InterviewContent { get; set; }
        public DateTime? Time { get; set; }
        public string Result { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }
  
    }
   
}