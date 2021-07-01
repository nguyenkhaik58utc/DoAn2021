using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileDiscipline
{
    public class HumanProfileDisciplineDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string NumberOfDecision { get; set; }
        public Nullable<System.DateTime> DateOfDecision { get; set; }
        public string Reason { get; set; }
        public int DisciplineCategoryID { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
    public class HumanProfileDisciplineResponse
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string NumberOfDecision { get; set; }
        public Nullable<System.DateTime> DateOfDecision { get; set; }
        public string Reason { get; set; }
        public string DisciplineCategoryName { get; set; }
    }
}
