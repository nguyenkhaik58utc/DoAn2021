using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileReward
{
    public class HumanProfileRewardDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string NumberOfDecision { get; set; }
        public Nullable<System.DateTime> DateOfDecision { get; set; }
        public string Reason { get; set; }
        public int AwardCategoryID { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class HumanProfileRewardResponse
    {
        public int ID { get; set; }
        public string NumberOfDecision { get; set; }
        public Nullable<System.DateTime> DateOfDecision { get; set; }
        public string Reason { get; set; }
        public string AwardCategoryName { get; set; }
    }

    public class HumanProfileRewardExcel
    {
        public int HumanEmployeeID { get; set; }
        public int NumberOfDecision { get; set; }
        public DateTime DateOfDecision { get; set; }
        public String Reason { get; set; }
        public String Name { get; set; }
    }
}

