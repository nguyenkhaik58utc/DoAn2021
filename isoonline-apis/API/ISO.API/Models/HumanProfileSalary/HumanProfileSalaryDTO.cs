using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileSalary
{
    public class HumanProfileSalaryDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Level { get; set; }
        public string Wage { get; set; }
        public DateTime DateOfApp { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Note { get; set; }
    }
}
