using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileTraining
{
    public class HumanProfileTrainingResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EducationTypeName { get; set; }
        public string Content { get; set; }
        public string Certificate { get; set; }
        public string EducationResultName { get; set; }
        public string Reviews { get; set; }
    }
}
