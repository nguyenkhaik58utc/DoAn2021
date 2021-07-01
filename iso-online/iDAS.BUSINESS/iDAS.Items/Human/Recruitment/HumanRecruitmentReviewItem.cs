using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentReviewItem
    {
        public int? ID { get; set; }
        public int ProfileID { get; set; }
        public int CriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public int? Point { get; set; }
        public System.DateTime? Time { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
  
    }
    public class RecruitmentProfileReviewItem
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public DateTime? Birthday { get; set; }
        public int ProfileID { get; set; }
        public string RequirementName { get; set; }
        public int? RequirementID { get; set; }
        public bool IsSend { get; set; }
    }
   
}